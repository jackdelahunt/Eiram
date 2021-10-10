using System.Collections.Generic;
using System.Linq;
using Eiram;
using Items;
using Items.Items;
using Registers;
using UnityEngine;

namespace Inventories
{
    public class PlayerInventory
    {
        public readonly int Slots = 9;
        public readonly List<ItemStack> ItemStacks;

        public PlayerInventory()
        {
            ItemStacks = Enumerable.Repeat(ItemStack.Empty, Slots).ToList();
        }

        public int TryAddItem(ItemId itemId, int size)
        {
            var item = Register.GetItemById(itemId);
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                // check for same item
                if (ItemStacks[i].ItemId == itemId)
                {
                    if (ItemStacks[i].Size < item.maxStack)
                    {
                        int total = ItemStacks[i].Size + size;
                        if (total > item.maxStack)
                        {
                            ItemStacks[i].Size = item.maxStack;
                            int remainder = total - item.maxStack;
                            return TryAddItem(itemId, remainder);
                        }
                        else
                        {
                            ItemStacks[i].Size = total;
                            return 0;
                        }
                    }
                }
            }
            
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                // check for empty slot
                if (ItemStacks[i] == ItemStack.Empty)
                {
                    ItemStacks[i] = new ItemStack(itemId, size);
                    return 0;
                }
            }

            return size;
        }

        public int Contains(ItemId itemId)
        {
            for (int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i].ItemId == itemId) 
                    return i;
            }

            return -1;
        }
        
        public int NextAvailableSlot(ItemId itemId, int maxStack)
        {
            for(int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i] == ItemStack.Empty || (ItemStacks[i].ItemId == itemId && ItemStacks[i].Size < maxStack)) 
                    return i;
            }
            
            return -1;
        }

        public int NextEmptySlot()
        {
            for(int i = 0; i < ItemStacks.Count; i++)
            {
                if (ItemStacks[i] == ItemStack.Empty) 
                    return i;
            }
            
            return -1;
        }
    }
}