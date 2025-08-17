using System;

namespace MPANAGTK.Frontend
{
    public struct IMPANAGUI
    {
        internal FoodDBView NutritionDBView
        {
            get;
            set;
        }
        /*
        internal IngredientDBView IngredientDBView
        {
            get;
            set;
        }*/
        internal RecipeDBView RecipeDBView
        {
            get;
            set;
        }
        internal RecipeIngredientDBView RecipeIngredientDBView
        {
            get;
            set;
        }
        internal CompoundIngredientsDBView CompoundIngredientsDBView
        {
            get;
            set;
        }
    }
}
