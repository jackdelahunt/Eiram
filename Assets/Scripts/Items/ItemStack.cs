using System;
using Eiram;

namespace Items
{
    [Serializable]
    public class ItemStack : IComparable<ItemStack>
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

        public int CompareTo(ItemStack other)
        {
            if (ReferenceEquals(this, other)) return 0;

            if (IsEmpty()) return 1;
            if (other.IsEmpty()) return -1;
            
            var itemIdComparison = ItemId.CompareTo(other.ItemId);
            if (itemIdComparison != 0) return itemIdComparison;

            if (Size > other.Size) 
                return -1;
            
            return 1;
        }
    }
}