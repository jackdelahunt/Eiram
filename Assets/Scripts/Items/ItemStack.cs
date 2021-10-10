using Eiram;

namespace Items
{
    public class ItemStack
    {
        public static readonly ItemStack Empty = new ItemStack(ItemId.UNKNOWN, 0);
        
        public ItemId ItemId;
        public int Size;

        public ItemStack(ItemId itemId, int size)
        {
            this.ItemId = itemId;
            this.Size = size;
        }

    }
}