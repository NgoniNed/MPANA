using System;
using System.Collections.Generic;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend.Database;

namespace MPANAGTK.Frontend
{
    public class SelectedIngredientsView : Table
    {
        private FoodDBView FoodDBref
        {
            get;
        }
        private ListStore seedInforList;
        public SelectedIngredientsView(ref FoodDBView foodDBView) : base(10, 1, false)
        {
            this.TooltipText = "Selected Ingredients View";
            FoodDBref = foodDBView;
            ScrolledWindow scrolledList = new ScrolledWindow();
            FoodDBref.SelectionCountChange += FoodDBref_SelectionCountChange;

            scrolledList.ShadowType = ShadowType.Out;
            scrolledList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            scrolledList.Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer colr = new CellRendererText();
            col.Title = "Ingredient Name";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);
            seedInforList = new ListStore(typeof(string));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            foreach (FoodNutrition info in FoodDBref.selectedOptionsF)
            {
                seedIter = seedInforList.AppendValues($"{info.Shrt_Desc}");
            }
            treeView.RowActivated += TreeView_RowActivated;
            Attach(scrolledList, 0, 1, 0, 9);
            Attach(ButtonsTable(), 0, 1, 9, 10);
        }

        private Table ButtonsTable()
        {
            Table buttonBox = new Table(1, 3, false);
            Button previewBtn = new Button("preview");
            previewBtn.Clicked += PreviewBtn_Clicked;

            Button modifyBtn = new Button("update");
            modifyBtn.Clicked += ModifyBtn_Clicked;

            buttonBox.Attach(previewBtn, 0, 1, 0, 1);
            buttonBox.Attach(modifyBtn, 1, 2, 0, 1);
            buttonBox.SetSizeRequest(280, 30);
            return buttonBox;
        }

        private void PreviewBtn_Clicked(object sender, EventArgs e)
        {
            /*
             * create a new window, pop up preferably
             * pass it the fooddbref.selectOptions
             * freeze current window till it is disposed
             */
            //Console.WriteLine($"There are currently {FoodDBref.selectedOptionsF.Count} selected");
            MPANA_PreviewWindow previewWindow = new MPANA_PreviewWindow(FoodDBref.selectedOptionsF);
            previewWindow.ShowAll();
        }

        private void FoodDBref_SelectionCountChange(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void ModifyBtn_Clicked(object sender, EventArgs e)
        {
            UpdateView();
        }

        public void UpdateView()
        {

            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();
            FoodDBref.selectedOptionsF = FoodDBref.DBReader.GetFoodsWithNutrition(FoodDBref.selectedOptions);
            foreach (var info in FoodDBref.selectedOptionsF)
            {

                seedIter = seedInforList.AppendValues($"{info.Shrt_Desc}");
            }
        }

        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            TreeView view = (TreeView)o;
            if (view.Model.GetIter(out iter, args.Path))
            {
                string row = (string)view.Model.GetValue(iter, 0);
                List<FoodNutrition> tmp = FoodDBref.DBReader.GetFoodsWithNutrition(FoodDBref.selectedOptions);
                foreach (FoodNutrition info in tmp)
                {
                    if (row.Equals($"{info.Shrt_Desc}"))
                    {
                        FoodDBref.selectedOptions.Remove(info.NDB_No);
                        break;
                    }
                }
            }
            UpdateView();
        }
    }
}