using System;
namespace MPANAGTK.Models
{
    [Serializable]
    public struct FoodNutritionStruct
    {
        public string Labels
        {
            get;
            set;
        }

        public string Values
        {
            get;
            set;
        }
    }
}
