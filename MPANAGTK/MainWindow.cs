using System;
using System.Collections.Generic;
using Gtk;
using MPANAGTK;
using MPANAGTK.Backend;
using MPANAGTK.Backend.Database;
using MPANAGTK.Frontend;

public partial class MainWindow : Gtk.Window
{
    private CSVReadFoodDB readFoodDB
    {
        get;
        set;
    }
    public Entry searchEntry;
    SelectedIngredientsView selectedView;
    public MainWindow(ProgressWindow progress,MPANAGTK.Backend.CSVReadFoodDB dataCollection, CSVReadRecipe dataCollection1, CSVReader_RecipeIngredientDB dataCollection2,CSVReadIngredients datacollection3, CSVReadCompoundIngredients cSVReadCompoundIngredients) : base(Gtk.WindowType.Toplevel)
    {
        Build();
        readFoodDB = dataCollection;
        Table multiwayViewTable = new Table(2, 2, false);
        Table searchTable = new Table(1, 3, false);
        Label searchlbl = new Label("Search");
        searchTable.Attach(searchlbl, 0, 1, 0, 1);
        searchEntry = new Entry();
        searchEntry.TextDeleted += SearchEntry_TextDeleted; ;
        searchTable.Attach(searchEntry, 1, 2, 0, 1);
        Button searchbtn = new Button("Search");
        searchbtn.Clicked += Searchbtn_Clicked;

        searchTable.Attach(searchbtn, 2, 3, 0, 1);
        searchTable.SizeAllocate(new Gdk.Rectangle(0, 0, 1280, 30));
        multiwayViewTable.Attach(searchTable, 0, 2, 0, 1);

        //multiwayViewTable.SetSizeRequest(1280,760);
        Table compoundViewArea1 = new Table(1, 1, false);
        compoundViewArea1.WidthRequest = 280;
        compoundViewArea1.HeightRequest = 700;
        foodDBView = new FoodDBView(readFoodDB);
        RecipeIngredientDBView recipeIngredientDBView = new RecipeIngredientDBView(dataCollection2, this);
        //IngredientDBView ingredientDBView = new IngredientDBView(datacollection3.Ingredient_db);
        CompoundIngredientsDBView compoundIngredientsDBView= new CompoundIngredientsDBView(cSVReadCompoundIngredients.Ingredient_db) { HeightRequest = 230 };
        RecipeDBView recipeDBView = new RecipeDBView(progress,dataCollection1, recipeIngredientDBView, compoundIngredientsDBView);
        FilterDropDownMenu filterMenu = new FilterDropDownMenu(foodDBView);
        VBox lvBox = new VBox();
        lvBox.PackStart(filterMenu,false,false,10);
        lvBox.PackStart(compoundIngredientsDBView, true, true, 5);
        //FilterMenu should have label, disabled entry area and drop down selection area
        compoundViewArea1.Attach(lvBox, 0, 1, 0, 1);
        multiwayViewTable.Attach(compoundViewArea1, 1, 2, 1, 2);

        Table fourWayDBViewArea = new Table(2, 2, true);
        fourWayDBViewArea.WidthRequest = 1000;
        fourWayDBViewArea.Attach(foodDBView, 0, 1, 0, 1);
        
        selectedView = new SelectedIngredientsView(ref foodDBView);

        fourWayDBViewArea.Attach(selectedView, 0, 1, 1, 2);

        fourWayDBViewArea.Attach(recipeDBView, 1, 2, 0, 1);
        fourWayDBViewArea.Attach(recipeIngredientDBView, 1, 2, 1, 2);

        multiwayViewTable.Attach(fourWayDBViewArea, 0, 1, 1, 2);
        
        Add(multiwayViewTable);
        ShowAll();
    }

    private void SearchEntry_TextDeleted(object o, TextDeletedArgs args)
    {
        foodDBView.ReloadView(readFoodDB);
    }

    private FoodDBView foodDBView;
    public void Searchbtn_Clicked(object sender, EventArgs e)
    {
        if(!searchEntry.Text.Equals(""))
        {
            this.Sensitive = false;
            int searchCount = 0;
            //int dbCounter = 0;
            string tmpSearch = searchEntry.Text;
            List<FoodNutritionWCategory> foundMatches = new List<FoodNutritionWCategory>();
            foreach(string subSearch in searchEntry.Text.Split(';'))
            {
                foreach (var fd_db in readFoodDB.FoodNutr_db)
                {
                    if (fd_db.FoodNutrition.Shrt_Desc.Replace(',', ' ').Contains(subSearch.Trim().ToUpper()))
                    {
                        searchCount++;
                        foundMatches.Add(fd_db);
                    }

                }
            }
            
            this.Sensitive = true;
            foodDBView.UpdateTreeView(foundMatches);
        }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
