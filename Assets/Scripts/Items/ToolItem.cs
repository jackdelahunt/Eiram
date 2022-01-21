using System;
using Events;
using Items.Items;
using Players;
using Tags;
using UnityEngine;

namespace Items
{
    public abstract class ToolItem : AbstractItem
    {
        public ToolItem(ConcreteItemData concreteItemData) : base(concreteItemData)
        {
#if UNITY_EDITOR
            if (!(concreteItemData is ToolItemData))
            {
                throw new Exception($"Tool item needs a tool item data, {concreteItemData.itemName}");
            }
#endif
            var toolItemData = concreteItemData as ToolItemData;
            defaultItemStack = new RichItemStack()
            {
                Size = 1,
                ItemId = concreteItemData.itemId,
                Tag = new Tag(("durability", toolItemData.durability))
            };
        }

        public override void OnBreak(Vector3Int worldPosition, ItemStack stack, Player player)
        {
            base.OnBreak(worldPosition, stack, player);
            Debug.Assert(stack is RichItemStack);
            OnToolBreak(worldPosition, stack as RichItemStack, player);
        }

        public virtual void OnToolBreak(Vector3Int worldPosition, RichItemStack stack,
            Player player)
        {
            if (stack.Tag.HasKey("durability"))
            {
                int durability = stack.Tag.GetInt("durability");
                stack.Tag.SetInt("durability", --durability);
                if(durability <= 0)
                {
                    EiramEvents.OnToolBreakEvent(stack);
                }
            }
            else
            {
                Debug.LogError("Tool item has no durability tag, cannot decrease");
            }
        }
    }
    
    public class WoodShovel : ToolItem
    {
        public WoodShovel(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class WoodPickaxe : ToolItem
    {
        public WoodPickaxe(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class WoodAxe : ToolItem
    {
        public WoodAxe(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
}