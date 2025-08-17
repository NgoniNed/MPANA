using System;
using System.Collections.Generic;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;

namespace MPANAGTK.Frontend
{
    [Obsolete]
    internal class IngredientDBView : ScrolledWindow
    {
        public Dictionary<string, IngredientsStruct> cSVRead;

        public IngredientDBView(Dictionary<string, IngredientsStruct> cSVRead)
        {
            this.TooltipText = "Ingredients Database View";
            this.cSVRead = cSVRead;
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer colr = new CellRendererText();
            col.Title = "Ingredient Name";
            col.MaxWidth = 166;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Synonyms Name";
            col.MaxWidth = 166;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 1);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Category Name";
            col.MaxWidth = 166;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 2);
            treeView.AppendColumn(col);
            seedInforList = new ListStore(typeof(string), typeof(string), typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();

            foreach (KeyValuePair<string, IngredientsStruct> info in cSVRead)
            {
                seedIter = seedInforList.AppendValues($"{info.Value.AliasedIngredientName}", $" {info.Value.IngredientSynonyms}", $"{info.Value.Category}");

            }

        }
        private ListStore seedInforList;
        internal void UpdateTreeView(List<IngredientsStruct> newIngredientsList)
        {
            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();

            foreach (IngredientsStruct info in newIngredientsList)
            {
                seedIter = seedInforList.AppendValues($"{info.AliasedIngredientName}", $" {info.IngredientSynonyms}", $"{info.Category}");

            }
        }
    }
}
