using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using Gtk;
using MPANAGTK.Backend;
using MPANAGTK.Backend.Database;
using GLib;
using MPANAGTK.Frontend;

namespace MPANAGTK
{
    class MainClass
    {
        private static ProgressWindow progressWindow;

        public static void Main(string[] args)
        {
            Application.Init();
            progressWindow = new ProgressWindow();
            progressWindow.ShowAll();
            Task.Run(() =>
            {
                try
                {
                    Idle.Add(() =>
                    {
                        Frontend.FoodChemistry.FoodChemistry foodChemist = new Frontend.FoodChemistry.FoodChemistry(progressWindow);
                        foodChemist.Show();
                        return false;
                    });

                    progressWindow.UpdateProgress("Starting Initialization and preprocessing...");
                    List<IMPANA> dataCollection  = InitializeDatabase();

                    
                    progressWindow.UpdateProgress("Finished Initialization and preprocessing...");
                    
                    Idle.Add(() => {
                        MainWindow win = new MainWindow(progressWindow,(CSVReadFoodDB)dataCollection[0], (CSVReadRecipe)dataCollection[1], (CSVReader_RecipeIngredientDB)dataCollection[4], (CSVReadIngredients)dataCollection[2], (CSVReadCompoundIngredients)dataCollection[3]);
                        win.Show();
                        return false; 
                    });


                }
                catch (Exception ex)
                {
                    progressWindow.UpdateProgress($"An error occurred: {ex.Message}");
                    

                }


            });
            Application.Run();
        }
        private static List<IMPANA> InitializeDatabase()
        {
            List<IMPANA> datafromDB = new List<IMPANA>();
            progressWindow.UpdateProgress($"{"config.json"} file exists {File.Exists("config.json")}");

            
            string dbFilePath = Config.Settings.Databases.DatabasePath; //"MPANAdb.db";

            bool dbExists = File.Exists(dbFilePath);
            progressWindow.UpdateProgress($"{dbFilePath} file exists {dbExists}");
            
            var csvFilePaths = new Dictionary<string, string>
            {
                { "FoodNutritionDB", Config.Settings.CSVSourceFile.FoodNutritionDB }, //csvFilePaths2.FoodNutritionDB },
                { "srFoodDescription", Config.Settings.CSVSourceFile.srFoodDescription }, //csvFilePaths2.srFoodDescription },
                { "RecipeDB_Recipe", Config.Settings.CSVSourceFile.RecipeDB_Recipe }, //csvFilePaths2.RecipeDB_Recipe },
                { "IngredientDB_Recipe",Config.Settings.CSVSourceFile.IngredientDB_Recipe }, //csvFilePaths2.IngredientDB_Recipe },
                { "CompoundIngredientDB_Recipe", Config.Settings.CSVSourceFile.CompoundIngredientDB_Recipe }, //csvFilePaths2.CompoundIngredientDB_Recipe },
                { "RecipeIngredientDB_Recipe", Config.Settings.CSVSourceFile.RecipeIngredientDB_Recipe }, //csvFilePaths2.RecipeIngredientDB_Recipe }
            };

            progressWindow.UpdateProgress($"Checking resource file paths");
            foreach(var keyvalue in csvFilePaths)
            {
                progressWindow.UpdateProgress($"Resource {keyvalue.Key} exists {File.Exists(keyvalue.Value)}");
            }
            var csvHashes = new Dictionary<string, string>();
            foreach (var csvFile in csvFilePaths)
            {
                csvHashes[csvFile.Key] = ComputeFileHash(csvFile.Value);
            }

            using (var context = DbContextFactory.CreateDbContext())
            {
                if (dbExists)
                {
                    var storedHashes = GetStoredHashesFromDatabase(context);

                    progressWindow.UpdateProgress($"Validating resources integrity and consistency");

                    bool consistent = true;
                    foreach (var hash in csvHashes)
                    {
                        bool hashKeyFound = storedHashes.ContainsKey(hash.Key);
                        bool hashIsNotMatch = storedHashes[hash.Key] != hash.Value;
                        progressWindow.UpdateProgress($"Key {hash.Key} is found {hashKeyFound}");
                        progressWindow.UpdateProgress($"Key {hash.Key} is not valid {hashIsNotMatch}");

                        if (!hashKeyFound || hashIsNotMatch)
                        {
                            consistent = false;
                            break;
                        }
                    }

                    if (consistent)
                    {
                        progressWindow.UpdateProgress("Loading from existing database");
                        datafromDB=Preprocessor();
                    }
                    else
                    {
                        progressWindow.UpdateProgress("Inconsistencies found. Deleting database.");
                        context.Database.EnsureDeleted();
                        progressWindow.UpdateProgress("Inconsistencies found. Creating database.");
                        context.Database.EnsureCreated();
                        progressWindow.UpdateProgress("Migrating database due to Inconsistencies.");
                        context.MigrateDatabase();
                        progressWindow.UpdateProgress("Updating database with file resource data.");
                        LoadDataIntoDatabase(context, csvFilePaths);
                        progressWindow.UpdateProgress("Starting preprocessing.");
                        datafromDB = Preprocessor();
                        progressWindow.UpdateProgress("Finishing preprocessing.");

                    }
                }
                else
                {
                    progressWindow.UpdateProgress("Creating database due to non-existence.");
                    context.Database.EnsureCreated();
                    progressWindow.UpdateProgress("Migrating database due to non-existence.");
                    context.MigrateDatabase();
                    progressWindow.UpdateProgress("Updating database with file resource data.");
                    LoadDataIntoDatabase(context, csvFilePaths);
                    progressWindow.UpdateProgress("Starting preprocessing.");
                    datafromDB = Preprocessor();
                    progressWindow.UpdateProgress("Finishing preprocessing.");
                }
            }

            return datafromDB;
        }
        private static void LoadDataIntoDatabase(MPANADbContext context, Dictionary<string, string> csvFilePaths)
        {
            foreach (var csvFile in csvFilePaths)
            {
                string hashValue = ComputeFileHash(csvFile.Value);
                var dataResource = new DataResource
                {
                    Title = csvFile.Key, 
                    Resource = csvFile.Value,
                    Active = true, 
                    HashValue = hashValue,
                    FileLocation = csvFile.Value
                };

                context.DataResources.Add(dataResource);
                context.SaveChanges(); 
            }
        }


        private static Dictionary<string, string> GetStoredHashesFromDatabase(MPANADbContext context)
        {
            var storedHashes = new Dictionary<string, string>();

            var dataResources = context.DataResources.ToList();

            foreach (var resource in dataResources)
            {
                storedHashes[resource.Title] = resource.HashValue;
            }

            return storedHashes;
        }


        private static string ComputeFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static List<IMPANA> Preprocessor()
        {
            using (var context = DbContextFactory.CreateDbContext())
            {

                progressWindow.UpdateProgress("Reading Food Database...");
                Backend.CSVReadFoodDB foodDb = new Backend.CSVReadFoodDB(progressWindow,"FoodNutritionDB", "FoodItem", context);
                progressWindow.UpdateProgress("Finished Food Database...");

                progressWindow.UpdateProgress("Reading Food Description...");
                Backend.CSVFoodDescription foodDescription = new Backend.CSVFoodDescription(progressWindow, "srFoodDescription", context);

                progressWindow.UpdateProgress("Reading Recipe Database...");
                Backend.CSVReadRecipe CulinaryRecipeDb = new Backend.CSVReadRecipe(progressWindow,"RecipeDB_Recipe", "Recipe", context);

                progressWindow.UpdateProgress("Reading Ingredients Database...");
                Backend.CSVReadIngredients CulinaryIngredientDb = new Backend.CSVReadIngredients(progressWindow,"IngredientDB_Recipe", "Ingredient", context);

                progressWindow.UpdateProgress("Reading Compound Ingredients Database...");
                Backend.CSVReadCompoundIngredients CulinaryCompoundIngredientDb = new Backend.CSVReadCompoundIngredients(progressWindow,"CompoundIngredientDB_Recipe", "CompoundI", context);

                progressWindow.UpdateProgress("Reading Recipe Ingredient Aliases Database... 200");
                Backend.CSVReader_RecipeIngredientDB CulinaryRecipeIngredientAliasDb = new Backend.CSVReader_RecipeIngredientDB(progressWindow,"RecipeIngredientDB_Recipe", "RI_Alias", context);


                List<IMPANA> dataCollection = new List<IMPANA>();
                dataCollection.Add(foodDb);
                dataCollection.Add(CulinaryRecipeDb);
                dataCollection.Add(CulinaryIngredientDb);
                dataCollection.Add(CulinaryCompoundIngredientDb);

                dataCollection.Add(CulinaryRecipeIngredientAliasDb);

                return dataCollection;
            }
        }

    }
}

/*
 * <Updated 7 May 2023>
 *      treeView updates need not redraw the whole view but update the iter values
 *      clear the ListStore and reload values into it
 * <\Updated 7 May 2023>
 * <Updated 7 May 2023>
 *      auto search trim white space ending
 * <Updated 7 May 2023>
 * <Updated 10 May 2023>
 *      issue with compound ingredient window regarding tool tip not showing full list of constituates of compound ingredeints
 *      Resolved. issue was due to initial csv reader only taking part of the constituent ingredients. Replaced csv read with regex string capture
 *      Modified VIew to use TreeStore to present compound ingredients constituent ingredeints
 * <\Updated 10 May 2023>
 * <Updated 11 May 2023>
 *      culinaryDb4 non standard csv formating resulted in wrongly extracting values for data structures. fixed
 * <\Updated 11 May 2023>
 * <Updated 14 May 2023>
 *      Successfull integration of food groups properties and resource file creation
 * <\Updated 14 May 2023>
 * <Updated 17 May 2023>
 *      add bmi-bmr section to preveiw window combine mpana calc
 * <Updated 17 May 2023>
 * 
 * link bmi-bmr and nutrient requirments.
 * parse of measurement aspects of foods using standard measurement form kg/g take advantage of gram weight
 * allow for changing of size of measurements to meet recipe targets or user options
 *<Updated 15 May 2023>
 *      stop processing of resource file for food nutrition database but rather parse the dictionary to the fooddesciption csv processing class
 * <\Updated 15 May 2023>
 * color red for excess and color blue for lack in total row for nutrient sums.
 * 
 * BMI R screen build
 * 
 * compare the csv and db entry
 */ 