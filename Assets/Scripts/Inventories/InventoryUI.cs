using Items;

namespace Inventories
{
    public interface InventoryUI
    {
        void ItemPopped(int itemSlot);
        void ItemPlaced(int itemSlot, ItemStack itemStack);
    }
}