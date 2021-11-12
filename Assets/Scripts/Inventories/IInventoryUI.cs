using Items;

namespace Inventories
{
    public interface IInventoryUI
    {
        void ItemPopped(int itemSlot);
        void ItemPlaced(int itemSlot, ItemStack itemStack);
    }
}