using System.Collections.Generic;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend.Database;

namespace MPANAGTK.Frontend
{

    internal class singleColumnTreeView : ScrolledWindow
    {
        public singleColumnTreeView(List<FoodNutrition> ingredientNames)
        {
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer colr = new CellRendererText();
            col.Title = "Shrt_Desc";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "GmWt_Desc1";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 2);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Weight";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 1);
            treeView.AppendColumn(col);

            ListStore seedInforList = new ListStore(typeof(string), typeof(int), typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            foreach (FoodNutrition info in ingredientNames)
            {
                seedIter = seedInforList.AppendValues($"{info.Shrt_Desc}", info.GmWt_1, $"{info.GmWt_Desc1}");
            }
        }

        public singleColumnTreeView(Dictionary<Models.srFoodDescription, List<Models.Nutrition>> ingredientNames)
        {
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer colr = new CellRendererText();
            col.Title = "Shrt_Desc";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "GmWt_Desc1";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 2);
            treeView.AppendColumn(col);

            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Weight";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 1);
            treeView.AppendColumn(col);

            ListStore seedInforList = new ListStore(typeof(string), typeof(int), typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            foreach (KeyValuePair<Models.srFoodDescription, List<Models.Nutrition>> info in ingredientNames)
            {
                seedIter = seedInforList.AppendValues($"{info.Key.Shrt_Desc}", info.Key.GmWt_1, $"{info.Key.GmWt_Desc1}");
            }
        }

    }
}