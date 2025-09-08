using System.Collections.Generic;
using System.Text;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;

namespace MPANAGTK.Frontend
{
    internal class RecipeDBView : ScrolledWindow
    {
        private CSVReadRecipe cSVReadRecipe;
        /*private IngredientDBView IngredientsDB
        {
            get;
            set;
        }*/
        private RecipeIngredientDBView RecipeIngredientDB
        {
            get;
            set;
        }
        /// <summary>
        /// obsolute
        /// </summary>
        /*private CompoundIngredientsDBView CompoundIngredientsDB
        {
            get;
            set;
        }*/

        public static ProgressWindow progressWindow
        {
            get;
            set;
        }

        public RecipeDBView(ProgressWindow progress, CSVReadRecipe cSVReadRecipe, RecipeIngredientDBView recipeIngredientDBView, CompoundIngredientsDBView compoundIngredientsDBView)
        {
            progressWindow = progress;
            //IngredientsDB = ingredientDBView;
            RecipeIngredientDB = recipeIngredientDBView;
            //CompoundIngredientsDB = compoundIngredientsDBView;
            this.cSVReadRecipe = cSVReadRecipe;
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRendererText colr = new CellRendererText();
            /*col.Title = "Recipe ID";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            */
            col.Title = "Recipe Name";
            col.PackStart(colr, true);
            colr.WrapWidth = 295;
            colr.WrapMode = Pango.WrapMode.Word;
            col.MaxWidth = 300;
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Cuisine Name";
            col.PackStart(colr, true);
            col.MaxWidth = 100;
            col.AddAttribute(colr, "text", 1);
            treeView.AppendColumn(col);

            seedInforList = new ListStore(typeof(string), typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            foreach (var info in cSVReadRecipe.Recipe_db)
            {
                //RecipeInformationStruct tmpryr = info.Key;
                seedIter = seedInforList.AppendValues($"{info.Title}", $"{info.Cuisine}");

            }
            treeView.RowActivated += TreeView_RowActivated;
        }
        private ListStore seedInforList;
        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            TreeView view = (TreeView)o;
            if (view.Model.GetIter(out iter, args.Path))
            {
                string rowCol1 = (string)view.Model.GetValue(iter, 0);
                //row1 now points to recipe name
                //retrieve the recipe id and name assorciated with this recipe entity
                var tmp = cSVReadRecipe.GetRecipeByOriginalName(rowCol1);

                RecipeIngredientDB.UpdateTreeView(tmp.RecipeID);
                //using the rowCol1 variable
                /*
                 * get the item pointed to by variable from the respective view
                 * so progress should display the datastructure info
                 */
                /*
                StringBuilder builder = new StringBuilder();
                StringBuilder builder1 = new StringBuilder();

                RecipeInformationStruct recipe;
                foreach (KeyValuePair<RecipeInformationStruct, List<string>> info in cSVReadRecipe)
                {
                    if(rowCol1.Equals(info.Key.Values[0]))
                    {
                        recipe = info.Key;
                        foreach(string tmp in info.Key.Values)
                        {
                            builder.AppendLine($"{tmp}");
                        }
                        break;
                    }
                }
                List<IngredientsStruct> newIngredientsList = new List<IngredientsStruct>();
                List<RecipeIngredientStruct> newRecipeIngredients = new List<RecipeIngredientStruct>();
                List<CompoundIngedientsStruct> newCompoundIngredients = new List<CompoundIngedientsStruct>();

                foreach (KeyValuePair<string, RecipeIngredientStruct> recingre in this.RecipeIngredientDB.cSVRead)
                {
                    progressWindow.UpdateProgress($"{recingre.Value.RecipeID}=>{recingre.Value.EntityID}=>{rowCol1}");

                    if (recingre.Value.RecipeID.Equals(rowCol1))
                    {

                        foreach (KeyValuePair<string,IngredientsStruct> ingre in this.IngredientsDB.cSVRead)
                        {
                            if(ingre.Value.EntityID.Equals(recingre.Value.EntityID))
                            {
                                //builder.AppendLine($"Ingredeints\n\t{ingre.Value.IngredientSynonyms}\n\t{ingre.Value.Category}");
                                newIngredientsList.Add(ingre.Value);
                            }
                        }
                        foreach (KeyValuePair<string, CompoundIngedientsStruct> compIngre in this.CompoundIngredientsDB.cSVRead)
                        {
                            if (compIngre.Value.EntityId.Equals(recingre.Value.EntityID))
                            {
                                //builder.AppendLine($"Compound Ingredeints\n\t{compIngre.Value.ContituentIngredients}\n\t{compIngre.Value.Category}\n\t{compIngre.Value.CompoundIngredientName}\n\t{compIngre.Value.CompoundIngredientSynonyms}");
                                newCompoundIngredients.Add(compIngre.Value);
                            }
                        }
                        //builder1.AppendLine($"\t\tRecipeIngredients\n\t\t{recingre.Value.OriginalIngredientName}\n\t\t{recingre.Value.AliasedIngredientName}\n\t\t{recingre.Value.RecipeID}\n\t\t{recingre.Value.EntityID}");
                        newRecipeIngredients.Add(recingre.Value);
                    }
                }
                builder.AppendLine(builder1.ToString());
                this.IngredientsDB.UpdateTreeView(newIngredientsList);
                this.RecipeIngredientDB.UpdateTreeView(newRecipeIngredients);
                this.CompoundIngredientsDB.UpdateTreeView(newCompoundIngredients);

                this.TooltipText = $"{builder.ToString()}";
                */
            }
        }
    }
}