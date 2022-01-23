using System;
using Items.Items;
using Players;
using Tags;
using UnityEngine;

namespace Items
{
    public class FoodItem : AbstractItem
    {
        public FoodItem(ConcreteItemData concreteItemData) : base(concreteItemData)
        {
#if UNITY_EDITOR
            if (!(concreteItemData is FoodItemData))
            {
                throw new Exception($"Food item needs a food item data, {concreteItemData.itemName}");
            }
#endif
        }

        public override bool OnUse(Vector3Int worldPosition, ItemStack stack, Player player)
        {
            base.OnUse(worldPosition, stack, player);
            return player.ChangeHunger(FoodItemData.hungerGain);
        }
        
        private FoodItemData FoodItemData => concreteItemData as FoodItemData;
    }

    public class CranberriesItem : FoodItem
    {
        public CranberriesItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
}