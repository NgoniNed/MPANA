using System;
namespace MPANAGTK
{
    [Serializable]
    public struct RecipeInformationStruct
    {
        public string[] Labels
        {
            get;
            set;
        }

        public string[] Values
        {
            get;
            set;
        }
    }
    [Serializable]
    public struct RecipeIngredientStruct
    {
        public string RecipeID
        {
            get;
            set;
        }
        public string OriginalIngredientName
        {
            get;
            set;
        }
        public string AliasedIngredientName
        {
            get;
            set;
        }
        public string EntityID
        {
            get;
            set;
        }

    }
    [Serializable]
    public struct IngredientsStruct
    {
        public string AliasedIngredientName
        {
            get;
            set;
        }
        public string IngredientSynonyms
        {
            get;
            set;
        }
        public string EntityID
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
    }
    [Serializable]
    public struct CompoundIngedientsStruct
    {
        public string CompoundIngredientName
        {
            get;
            set;
        }
        public string CompoundIngredientSynonyms
        {
            get;
            set;
        }
        public string EntityId
        {
            get;
            set;
        }
        public string ContituentIngredients
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }

    }
    public enum RecipeLabels
    {
        RecipeID,
        Title,
        Cuisine
    }
}
