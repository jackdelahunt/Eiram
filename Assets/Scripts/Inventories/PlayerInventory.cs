using System;
using System.Collections.Generic;
using System.Linq;
using Eiram;
using Events;
using Items;
using Items.Items;
using Registers;
using UnityEngine;

namespace Inventories
{
    public class PlayerInventory
    {
        public static readonly int Slots = 70;
        public readonly List<ItemStack> ItemStacks;
        public bool IsDirty = false;

        private int selectedSlot = 0;
        private const int hotbarSlotsCount = 10;
        
        public PlayerInventory()
        {
            ItemStacks = Enumerable.Repeat(ItemStack.Empty, Slots).ToList();
        }

        public int TryAddItem(ItemId itemId, int size)
        {
            void _addToStack(ItemId id, int slot, int newSize)
            {
                if(id == ItemId.UNKNOWN)
                    ItemStacks[slot].Size = newSize;
                else
                    ItemStacks[slot] = new ItemStack(id, newSize);
                
                IsDirty = true;
            }

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
                            _addToStack(ItemId.UNKNOWN, i, item.maxStack);
                            int remainder = total - item.maxStack;
                            return TryAddItem(itemId, remainder);
                        }
                        else
                        {
                            _addToStack(ItemId.UNKNOWN, i, total);
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
                    _addToStack(itemId, i, size);
                    return 0;
                }
            }

            return size;
        }

        public ItemStack RemoveFromItemStack(int slotIndex, int amount = 1)
        {
            var stack = ItemStacks[slotIndex];
            if (ItemStacks[slotIndex] != ItemStack.Empty)
            {
                IsDirty = true;
                if (amount > stack.Size)
                {
                    ItemStacks[slotIndex] = ItemStack.Empty;
                    return stack;
                }
                
                stack.Size -= amount;
                if(stack.Size == 0)
                    ItemStacks[slotIndex] = ItemStack.Empty;
                return new ItemStack(stack.ItemId, amount);
            }

            return ItemStack.Empty;
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

        public void SelectNext()
        {
            selectedSlot++;
            if (selectedSlot >= hotbarSlotsCount) selectedSlot = 0;
            EiramEvents.SelectedSlotChanged(this, selectedSlot);
        }
        
        public void SelectPrevious()
        {
            selectedSlot--;
            if (selectedSlot < 0) selectedSlot = hotbarSlotsCount - 1;
            EiramEvents.SelectedSlotChanged(this, selectedSlot);
        }

        public ItemStack PopSelectedItem()
        {
            return RemoveFromItemStack(selectedSlot, 1);
        }
    }
}