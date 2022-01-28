using System;
using System.Collections.Generic;
using System.Linq;
using Eiram;
using Events;
using Items;
using Recipes;
using Registers;
using static Eiram.Handles;

namespace Inventories
{
    [Serializable]
    public abstract class Inventory
    {
        private readonly int numberOfSlots;
        public readonly List<ItemStack> ItemStacks;
        public bool IsDirty = false;
        
        public Inventory(int numberOfSlots)
        {
            this.numberOfSlots = numberOfSlots;
            ItemStacks = new List<ItemStack>(numberOfSlots);
            for (int i = 0; i < numberOfSlots; i++)
            {
                ItemStacks.Add(new ItemStack());
            }
            IsDirty = true;
        }

        public int TryAddItem(ItemId itemId, int size)
        {
            var item = Register.GetItemByItemId(itemId);
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                // check for same item
                if (ItemStacks[i].ItemId == itemId)
                {
                    if (ItemStacks[i].Size < item.MaxStack())
                    {
                        int total = ItemStacks[i].Size + size;
                        if (total > item.MaxStack())
                        {
                            SetStack(itemId, i, item.MaxStack());
                            int remainder = total - item.MaxStack();
                            return TryAddItem(itemId, remainder);
                        }
                        else
                        {
                            SetStack(itemId, i, total);
                            return 0;
                        }
                    }
                }
            }
            
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                // check for empty slot
                if (ItemStacks[i].IsEmpty())
                {
                    SetStack(itemId, i, size);
                    return 0;
                }
            }

            return size;
        }
        public ItemStack RemoveFromItemStack(int slotIndex, int amount = 1)
        {
            var stack = ItemStacks[slotIndex]; 
            if (!stack.IsEmpty())
            {
                IsDirty = true;
                if (amount > stack.Size)
                {
                    ItemStacks[slotIndex].Empty();
                    return stack;
                }
                
                stack.Size -= amount;
                
                var toReturn = new ItemStack(stack.ItemId, amount); 
                
                if(stack.Size == 0)
                    ItemStacks[slotIndex].Empty();

                return toReturn;
            }

            return new ItemStack();
        }

        public int CountOf(ItemId itemId)
        {
            int total = 0;
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i].ItemId == itemId) 
                    total += ItemStacks[i].Size;
            }

            return total;
        }

        public ItemStack ItemStackAt(int slotIndex)
        {
            return ItemStacks[slotIndex];
        }

        public int NextAvailableSlot(ItemId itemId, int maxStack)
        {
            for(int i = 0; i < ItemStacks.Count; i++)
            {
                var currentStack = ItemStacks[i];
                if (currentStack.IsEmpty() || (currentStack.ItemId == itemId && currentStack.Size < maxStack)) 
                    return i;
            }
            
            return -1;
        }

        public int NextEmptySlot()
        {
            for(int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i].IsEmpty()) 
                    return i;
            }
            
            return -1;
        }

        public void ClearSlot(int slotIndex)
        {
            ItemStacks[slotIndex] = new ItemStack();
            IsDirty = true;
        }

        public Option<int> SlotOfStack(ItemStack itemStack)
        {
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i].Equals(itemStack))
                    return i;
            }

            return None<int>();
        }

        public int TotalOfItem(ItemId id)
        {
            int total = 0;
            foreach (var itemStack in ItemStacks)
            {
                if (itemStack.ItemId == id)
                    total += itemStack.Size;
            }

            return total;
        }
        
        public List<int> SlotsOfItem(ItemId id)
        {
            var slots = new List<int>();
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i].ItemId == id)
                    slots.Add(i);
            }

            return slots;
        }

        public bool CanBuildRecipe(BuildingRecipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Amount > TotalOfItem(ingredient.ItemId))
                    return false;
            }

            return true;
        }

        public void Sort()
        {
            ItemStacks.Sort();
            IsDirty = true;
        }
        
        private void SetStack(ItemId itemId, int slotIndex, int newSize)
        {
            ItemStacks[slotIndex] = Register.GetItemByItemId(itemId).DefaultItemStack();
            ItemStacks[slotIndex].Size = newSize;
            IsDirty = true;
        }
    }
}