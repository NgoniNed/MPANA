using System;
namespace MPANAGTK.Models
{
    [Serializable]
    public struct Nutrition
    {
        public MicroNutrition Name
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }
    }
}
