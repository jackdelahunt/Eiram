using System;
using Eiram;
using UnityEngine;

namespace Recipes
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/BuildingRecipe")]
    public class BuildingRecipe : ScriptableObject
    {
        public ItemCountPair FinalItem;
        public ItemCountPair[] Ingredients;
    }

    [Serializable]
    public class ItemCountPair
    {
        public ItemId ItemId;
        public int Amount;
    }
}