using System.Collections.Generic;
using MPANAGTK;
using Gtk;
using MPANAGTK.Backend.Database;

namespace MPANAGTK.Frontend
{

    public class MPANA_PreviewWindow : Window
    {
        public MPANA_PreviewWindow(List<FoodNutrition> selectedOptions) : base(WindowType.Toplevel)
        {
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            Table previewTable = new Table(1, 2, true);
            DefaultWidth = 1280;
            DefaultHeight = 700;
            previewTable.Attach(new singleColumnTreeView(selectedOptions), 0, 1, 0, 1);
            previewTable.Attach(new nutritionColumnTreeView(selectedOptions), 1, 2, 0, 1);

            /*
             * first collumn list of ingredient names
             * second column list of non numeric ingredient weight
             * third column numeric ingredient weight infor
             * forth cloumn is table with fnv of ingredient
             * 
             */
            scrolledWindow.AddWithViewport(previewTable);
            //scrolledWindow.Add(previewTable);
            Table previewMain = new Table(2, 1, true);
            previewMain.Attach(scrolledWindow, 0, 1, 0, 1);
            previewMain.Attach(new BMI_RViewPoint(), 0, 1, 1, 2);
            Add(previewMain);
        }
    }
}