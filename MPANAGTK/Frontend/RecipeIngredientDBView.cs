using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;

namespace MPANAGTK.Frontend
{

    public class RecipeIngredientDBView : ScrolledWindow
    {
        [Obsolete]public Dictionary<string, RecipeIngredientStruct> cSVRead;

        private MainWindow refOfSearchEntry
        {
            get;
            set;
        }
        public CSVReader_RecipeIngredientDB InstanceOfRecipeDB { get; }

        public RecipeIngredientDBView(CSVReader_RecipeIngredientDB dataCollection2, MainWindow searchEntry)
        {
            this.InstanceOfRecipeDB = dataCollection2;

            this.TooltipText = "Recipe Ingredient Database View";
            //this.cSVRead = InstanceOfRecipeDB.RecipeIngredient_db;//dataCollection2.RecipeIngredient_db;

            refOfSearchEntry = searchEntry;

            ShadowType = ShadowType.EtchedIn;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRendererText colr = new CellRendererText();
            /*col.Title = "Recipe ID";
            col.MaxWidth = 166;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            */
            col.Title = "Original Ingredient Name";
            //col.MaxWidth = 166;

            colr.WrapMode = Pango.WrapMode.WordChar;
            colr.WrapWidth = 500;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);
            /*
            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Aliased Ingredient Name";
            col.MaxWidth = 166;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 2);
            treeView.AppendColumn(col);
            */
            //seedInforList = new ListStore(typeof(string), typeof(string), typeof(string));
            seedInforList = new ListStore(typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();

            foreach (KeyValuePair<string, RecipeIngredientStruct> info in InstanceOfRecipeDB.RecipeIngredient_db)
            {
                //seedIter = seedInforList.AppendValues($"{info.Value.RecipeID}", $" {info.Value.OriginalIngredientName}", $"{info.Value.AliasedIngredientName}");
                seedIter = seedInforList.AppendValues( $" {info.Value.OriginalIngredientName}");
            }

            treeView.RowActivated += TreeView_RowActivated;
        }
        private ListStore seedInforList;
        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            //refOfSearchEntry;
            TreeIter iter;
            TreeView view = (TreeView)o;
            if (view.Model.GetIter(out iter, args.Path))
            {
                //string row = (string)view.Model.GetValue(iter, 2);
                string row = (string)view.Model.GetValue(iter, 0);
                //request obect from db
                Console.WriteLine(row);
                foreach(var alias in InstanceOfRecipeDB.GetRecipeAliasByOriginalName(row))
                {
                    if(alias.RecipeID.Equals(RecipeId))
                    {
                        refOfSearchEntry.searchEntry.Text = $"{alias.AliasedIngredientName};";
                    }
                }
                //refOfSearchEntry.searchEntry.Text = row.Trim().Replace(' ', ';');

                /*foreach (KeyValuePair<string, RecipeIngredientStruct> info in cSVRead)
                {
                    if (row.Contains($"{info.Value.AliasedIngredientName}"))
                    {
                    }
                }*/
            }
            refOfSearchEntry.Searchbtn_Clicked(null, null);
        }
        //consider obsulting to reduce inmemory requirements
        [Obsolete]
        internal void UpdateTreeView(List<RecipeIngredientStruct> newIngredientsList)
        {
            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();

            foreach (RecipeIngredientStruct info in newIngredientsList)
            {
                //seedIter = seedInforList.AppendValues($"{info.RecipeID}", $" {info.OriginalIngredientName}", $"{info.AliasedIngredientName}");
                seedIter = seedInforList.AppendValues($" {info.OriginalIngredientName}");

            }
        }
        private string RecipeId
        {
            get;
            set;
        }
        internal void UpdateTreeView(string rowCol1)
        {
            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();
            RecipeId = rowCol1;
            foreach (var info in InstanceOfRecipeDB.SearchUsingRecipeID(rowCol1))
            {
                //seedIter = seedInforList.AppendValues($"{info.RecipeID}", $" {info.OriginalIngredientName}", $"{info.AliasedIngredientName}");
                seedIter = seedInforList.AppendValues($" {info.OriginalIngredientName}");
                //Console.WriteLine($"{info.RecipeID}\t=\t{info.AliasedIngredientName}");
            }
        }
    }
}