using System;
namespace MPANAGTK.Backend
{
    public class AppConfig
    {
        public AppAsserts AppAsserts
        {
            get;
            set;
        }
        public Databases Databases
        {
            get;
            set;
        }
        public XMLExports XMLExports
        {
            get;
            set;
        }
        public CSVSourceFile CSVSourceFile
        {
            get;
            set;
        }
    }
    public class AppAsserts
    {
        public string LogoPath
        {
            get;
            set;
        }
    }
    public class Databases
    {
        public string DatabasePath
        {
            get;
            set;
        }

        public string FoodChemistryDBPath
        {
            get;
            set;
        }
    }
    public class XMLExports
    {
        public string CompoundAlternateParent
        {
            get;
            set;
        }
        public string CompoundSubstituents
        {
            get;
            set;
        }
        public string CompoundsEnzymes
        {
            get;
            set;
        }
        public string CompoundsFlavors
        {
            get;
            set;
        }
        public string CompoundsHealthEffects
        {
            get;
            set;
        }
        public string Enzymes
        {
            get;
            set;
        }
        public string Flavors
        {
            get;
            set;
        }
        public string HealthEffects
        {
            get;
            set;
        }
        public string FoodTaxonomies
        {
            get;
            set;
        }
        public string Foods
        {
            get;
            set;
        }
        public string Nutrients
        {
            get;
            set;
        }
    }
    public class CSVSourceFile
    {
        public string FoodNutritionDB
        {
            get;
            set;
        }

        public string srFoodDescription
        {
            get;
            set;
        }

        public string RecipeDB_Recipe
        {
            get;
            set;
        }
        public string IngredientDB_Recipe
        {
            get;
            set;
        }
        public string CompoundIngredientDB_Recipe
        {
            get;
            set;
        }
        public string RecipeIngredientDB_Recipe
        {
            get;
            set;
        }
        public string CategoryCsvUrlPath
        {
            get;
            set;
        }
        public string catcodenameurlpath
        {
            get;
            set;
        }
    }
}
