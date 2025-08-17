using System;
namespace MPANAGTK.Models
{
    [Serializable]
    public struct FoodNutritionLabels
    {
        public int NDB_No
        {
            get;
            set;
        }

        public string Shrt_Desc
        {
            get;
            set;
        }

        public int GmWt_1
        {
            get;
            set;
        }

        public string GmWt_Desc1
        {
            get;
            set;
        }

        public int Refuse_Pct
        {
            get;
            set;
        }
    }
}
