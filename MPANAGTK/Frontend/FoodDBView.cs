using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;
using MPANAGTK.Backend.Database;

namespace MPANAGTK.Frontend
{
    public class FoodDBView : ScrolledWindow
    {
        //private List<FoodNutritionWCategory> cSVRead;
        public List<int> selectedOptions;
        private int SelectionCounter = 0;
        public int SelectedOptionsCount
        {
            get
            {
                return SelectionCounter;
            }
            set
            {
                SelectionCounter = value;
                OnSelectionCountChange();
            }
        }

        internal void AppliedFilter_Changed(object sender, EventArgs e)
        {
            Entry filterEntry = (Entry)sender;
            string[] filterInfo = filterEntry.Text.Replace("=>", ",").Split(',');
            List<FoodNutritionWCategory> filterResults = DBReader.GetFilterNDBs(filterInfo[0]);
            UpdateTreeView(filterResults);
        }

        public List<FoodNutrition> selectedOptionsF { get; set; }
        public CSVReadFoodDB DBReader { get; }

        public event EventHandler SelectionCountChange;

        protected virtual void OnSelectionCountChange()
        {
            if (SelectionCountChange != null) SelectionCountChange(this, EventArgs.Empty);
        }

        private ListStore seedInforList;

        public FoodDBView(CSVReadFoodDB cSVRead)
        {
            this.DBReader = cSVRead;
            this.selectedOptionsF = new List<FoodNutrition>();
            this.TooltipText = "Food Database View";
            selectedOptions = new List<int>();
            //this.cSVRead = cSVRead.FoodNutr_db;
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRendererText colr = new CellRendererText();
            col.Title = "Weight Measurement";
            colr.WrapWidth = 75;
            colr.WrapMode = Pango.WrapMode.Word;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);
            col = new TreeViewColumn();
            colr = new CellRendererText();
            col.Title = "Ingredient Name";
            colr.Width = 205;
            colr.WrapWidth = 200;
            colr.WrapMode = Pango.WrapMode.Word;
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 2);
            treeView.AppendColumn(col);
            col = new TreeViewColumn();
            colr = new CellRendererText();
            colr.WrapWidth = 75;
            colr.WrapMode = Pango.WrapMode.Word;
            col.Title = "Food Group Name";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 1);
            treeView.AppendColumn(col);
            seedInforList = new ListStore(typeof(string), typeof(string), typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            foreach (var info in this.DBReader.FoodNutr_db)
            {
                //System.Diagnostics.Debug.WriteLine($"{info.Key.FdGrp_Cd}\t{info.Key.NDB_No}\t{info.Key.Shrt_Desc}\t");
                seedIter = seedInforList.AppendValues(info.FoodNutrition.GmWt_Desc1, info.FoodCategory, info.FoodNutrition.Shrt_Desc);
            }
            treeView.RowActivated += TreeView_RowActivated;
        }

        internal string[] FilterOptionsList()
        {
            return DBReader.FoodCategoryFilters;
        }

        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            TreeView view = (TreeView)o;
            if (view.Model.GetIter(out iter, args.Path))
            {
                string row = (string)view.Model.GetValue(iter, 2);
                StringBuilder builder = new StringBuilder();
                foreach (var info in DBReader.FoodNutr_db)
                {
                    if (row.Equals(info.FoodNutrition.Shrt_Desc))
                    {
                        if (!selectedOptions.Contains(info.FoodNutrition.NDB_No))
                        {
                            selectedOptions.Add(info.FoodNutrition.NDB_No);
                            SelectedOptionsCount = selectedOptions.Count;
                        }
                    }
                }
            }
        }

        internal void UpdateTreeView(List<FoodNutritionWCategory> foundMatches)
        {

            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();
            foreach (FoodNutritionWCategory info in foundMatches)
            {
                seedIter = seedInforList.AppendValues(info.FoodNutrition.GmWt_Desc1, info.FoodCategory, info.FoodNutrition.Shrt_Desc);
            }
            ShowAll();

        }

        internal void ReloadView(CSVReadFoodDB readFoodDB)
        {

            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();
            foreach (var info in readFoodDB.FoodNutr_db)
            {
                seedIter = seedInforList.AppendValues(info.FoodNutrition.GmWt_Desc1, info.FoodCategory, info.FoodNutrition.Shrt_Desc);
            }
            ShowAll();
        }
    }
}
