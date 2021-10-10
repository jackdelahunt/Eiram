using System.Collections.Generic;
using System.Linq;
using Eiram;
using Items;
using Items.Items;
using Registers;

namespace Inventories
{
    public class PlayerInventory
    {
        public const int Slots = 5;
        public readonly List<ItemStack> ItemStacks;

        public PlayerInventory()
        {
            ItemStacks = Enumerable.Repeat(ItemStack.Empty, Slots).ToList();

            AddItem(ItemId.DIRT, 20);
        }

        public bool AddItem(ItemId itemId, int size)
        {
            var insertIndex = Contains(itemId);
            if (insertIndex >= 0)
            {
                ItemStacks[insertIndex].Size += size;
                return true;
            }

            insertIndex = NextFreeSlot();
            if (insertIndex >= 0)
            {
                ItemStacks[insertIndex] = new ItemStack(itemId, size);
                return true;
            }

            return false;
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

        public int NextFreeSlot()
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