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
    [Serializable]
    public class PlayerInventory : Inventory
    {
        public static readonly int Slots = 70;

        private int selectedSlot = 0;
        private const int hotbarSlotsCount = 10;
        
        public PlayerInventory() : base(Slots) {}

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