using System;
using Eiram;
using Registers;
using Tags;

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

        public static ItemStack New(ItemId id, int amount)
        {
            var stack = Register.GetItemByItemId(id).DefaultItemStack();
            stack.Size = amount;
            return stack;
        }
        
        public virtual object Clone()
        {
            return new ItemStack()
            {
                ItemId = this.ItemId,
                Size = this.Size
            };
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

        public bool CanFit(ItemStack other)
        {
            if (ItemId != other.ItemId) 
                return false;

            var thisItem = Register.GetItemByItemId(ItemId);
            return thisItem.MaxStack() > Size + other.Size;
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

    [Serializable]
    public class RichItemStack : ItemStack
    {
        public Tag Tag;
        
        public override object Clone()
        {
            return new RichItemStack()
            {
                ItemId = this.ItemId,
                Size = this.Size,
                Tag = this.Tag.Clone() as Tag
            };
        }
    }
}