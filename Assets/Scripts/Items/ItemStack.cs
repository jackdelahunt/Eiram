using System;
using Eiram;

namespace Items
{
    [Serializable]
    public class ItemStack
    {
        public ItemId ItemId;
        public int Size;
        
        public ItemStack()
        {
            this.ItemId = ItemId.UNKNOWN;
            this.Size = 0;
        }

        public ItemStack(ItemId itemId, int size)
        {
            this.ItemId = itemId;
            this.Size = size;
        }

        public void Empty()
        {
            this.Size = 0;
            this.ItemId = ItemId.UNKNOWN;
        }

        public bool IsEmpty()
        {
            return (this.Size == 0 && this.ItemId == ItemId.UNKNOWN);
        }

        public override string ToString()
        {
            return $"{Size} : {ItemId}";
        }
    }
}