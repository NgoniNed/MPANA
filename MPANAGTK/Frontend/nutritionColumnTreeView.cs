using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend.Database;
using MPANAGTK.Models;

namespace MPANAGTK.Frontend
{

    internal class nutritionColumnTreeView : ScrolledWindow
    {
        public nutritionColumnTreeView(List<FoodNutrition> selectedOptions)
        {
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col;
            CellRenderer colr;

            int i = 0;
            StringBuilder builder1 = new StringBuilder();
            string[] mkl = new string[Enum.GetNames(typeof(MicroNutrition)).Length];

            foreach (string tmptm in Enum.GetNames(typeof(MicroNutrition)))
            {

                col = new TreeViewColumn();
                colr = new CellRendererText();
                mkl[i] = tmptm;
                col.Title = $"{tmptm}";
                col.PackStart(colr, true);
                col.AddAttribute(colr, "text", i++);
                treeView.AppendColumn(col);
            }
            ListStore seedInforList = new ListStore(Type.GetTypeArray(mkl));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            Nutrition[] totatNut = new Nutrition[mkl.Length];
            for (i = 0; i < mkl.Length; i++)
            {
                totatNut[i] = new Nutrition() { Name = (MicroNutrition)i, Value = 0 };

            }
            foreach (FoodNutrition info in selectedOptions)
            {
                //List<string> nutritions = new List<string>();
                StringBuilder builder = new StringBuilder();
                i = 0;
                foreach (int nutrTmp in info.ToArray())
                {
                    builder.Append($"{nutrTmp},");
                    totatNut[i].Value += nutrTmp;
                    //nutritions.Add(nutrTmp.Value.ToString());
                    i++;
                }
                seedIter = seedInforList.AppendValues(builder.ToString().TrimEnd(',').Split(','));
            }
            string[] vs = new string[totatNut.Length];
            i = 0;
            foreach (Nutrition tm in totatNut)
            {
                vs[i] = tm.Value.ToString();
                i++;
            }
            seedIter = seedInforList.AppendValues(vs);

        }

        public nutritionColumnTreeView(Dictionary<srFoodDescription, List<Nutrition>> selectedOptions)
        {
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col;
            CellRenderer colr;

            int i = 0;
            StringBuilder builder1 = new StringBuilder();
            string[] mkl = new string[Enum.GetNames(typeof(MicroNutrition)).Length];

            foreach (string tmptm in Enum.GetNames(typeof(MicroNutrition)))
            {

                col = new TreeViewColumn();
                colr = new CellRendererText();
                mkl[i] = tmptm;
                col.Title = $"{tmptm}";
                col.PackStart(colr, true);
                col.AddAttribute(colr, "text", i++);
                treeView.AppendColumn(col);
            }
            ListStore seedInforList = new ListStore(Type.GetTypeArray(mkl));
            treeView.Model = seedInforList;
            TreeIter seedIter = new TreeIter();
            Nutrition[] totatNut = new Nutrition[mkl.Length];
            for (i = 0; i < mkl.Length; i++)
            {
                totatNut[i] = new Nutrition() { Name = (MicroNutrition)i, Value = 0 };

            }
            foreach (KeyValuePair<srFoodDescription, List<Nutrition>> info in selectedOptions)
            {
                //List<string> nutritions = new List<string>();
                StringBuilder builder = new StringBuilder();
                i = 0;
                foreach (Nutrition nutrTmp in info.Value)
                {
                    builder.Append($"{nutrTmp.Value},");
                    totatNut[i].Value += nutrTmp.Value;
                    //nutritions.Add(nutrTmp.Value.ToString());
                    i++;
                }

                seedIter = seedInforList.AppendValues(builder.ToString().TrimEnd(',').Split(','));
            }
            string[] vs = new string[totatNut.Length];
            i = 0;
            foreach (Nutrition tm in totatNut)
            {
                vs[i] = tm.Value.ToString();
                i++;
            }
            seedIter = seedInforList.AppendValues(vs);

        }
    }
}