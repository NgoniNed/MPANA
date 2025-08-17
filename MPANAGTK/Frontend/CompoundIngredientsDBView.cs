using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;

namespace MPANAGTK.Frontend
{

    internal class CompoundIngredientsDBView : ScrolledWindow
    {
        public Dictionary<string, CompoundIngedientsStruct> cSVRead;
        private TreeStore seedInforList;
        public CompoundIngredientsDBView(Dictionary<string, CompoundIngedientsStruct> cSVRead)
        {
            this.TooltipText = "Compound Ingredient View";
            this.cSVRead = cSVRead;
            this.SetSizeRequest(200, 600);
            ShadowType = ShadowType.Out;
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);


            TreeView treeView = new TreeView();
            Add(treeView);
            TreeViewColumn col = new TreeViewColumn();
            CellRenderer colr = new CellRendererText();
            col.Title = "Compound Ingredient Name";
            col.PackStart(colr, true);
            col.AddAttribute(colr, "text", 0);
            treeView.AppendColumn(col);

            seedInforList = new TreeStore(typeof(string));

            TreeIter seedIter = new TreeIter();

            foreach (KeyValuePair<string, CompoundIngedientsStruct> info in cSVRead)
            {
                seedIter = seedInforList.AppendValues(info.Value.CompoundIngredientName);
                foreach (string constituentsIngr in info.Value.ContituentIngredients.Split(','))
                {
                    seedInforList.AppendValues(seedIter, constituentsIngr);
                }


            }
            treeView.Model = seedInforList;

            //treeView.RowActivated += TreeView_RowActivated;

        }

        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            TreeView view = (TreeView)o;
            if (view.Model.GetIter(out iter, args.Path))
            {
                string rowCol1 = (string)view.Model.GetValue(iter, 0);
                StringBuilder builder = new StringBuilder();
                foreach (KeyValuePair<string, CompoundIngedientsStruct> info in cSVRead)
                {
                    if (rowCol1.Equals(info.Value.CompoundIngredientName))
                    {
                        System.Diagnostics.Debug.WriteLine(info.Value.ContituentIngredients);

                        builder.AppendLine($"\n\t\t{info.Value.CompoundIngredientName}\n\t\t{info.Value.CompoundIngredientSynonyms}\n\t\t{info.Value.ContituentIngredients}\n\t\t{info.Value.Category}");
                        foreach (string tmp in info.Value.ContituentIngredients.Split(','))
                        {
                            builder.AppendLine(tmp);
                        }
                        //break;
                    }
                }
                System.Diagnostics.Debug.WriteLine(builder.ToString());
            }
        }

        internal void UpdateTreeView(List<CompoundIngedientsStruct> newCompoundIngredients)
        {
            seedInforList.Clear();
            TreeIter seedIter = new TreeIter();

            foreach (CompoundIngedientsStruct info in newCompoundIngredients)
            {
                seedIter = seedInforList.AppendValues(info.CompoundIngredientName);
                foreach (string constituentsIngr in info.ContituentIngredients.Split(','))
                {
                    seedInforList.AppendValues(seedIter, constituentsIngr);
                }


            }
        }
    }
}