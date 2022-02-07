using System;

namespace Inventories
{
    [Serializable]
    public class ChestInventory : Inventory
    {
        public static readonly int Slots = 60;

        public ChestInventory() : base(Slots)
        {
        }
    }
}