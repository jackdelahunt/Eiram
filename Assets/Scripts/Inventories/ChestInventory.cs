using System;

namespace Inventories
{
    [Serializable]
    public class ChestInventory : Inventory
    {
        public static readonly int Slots = 70;

        public ChestInventory() : base(Slots)
        {
        }
    }
}