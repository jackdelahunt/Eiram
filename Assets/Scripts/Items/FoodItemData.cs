using Eiram;
using Items.Items;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/FoodItem")]
    public class FoodItemData : ConcreteItemData
    {
        public int hungerGain;
    }
}