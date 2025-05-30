﻿using System;
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
        public static readonly int Slots = 60;

        public int SelectedSlot { get; private set; }
        private const int hotbarSlotsCount = 10;

        public PlayerInventory() : base(Slots)
        {
            SelectedSlot = 0;
        }

        public void SelectNext()
        {
            SelectedSlot++;
            if (SelectedSlot >= hotbarSlotsCount) SelectedSlot = 0;
            EiramEvents.OnSelectedSlotChangedEvent(SelectedSlot);
        }
        
        public void SelectPrevious()
        {
            SelectedSlot--;
            if (SelectedSlot < 0) SelectedSlot = hotbarSlotsCount - 1;
            EiramEvents.OnSelectedSlotChangedEvent(SelectedSlot);
        }

        public ItemStack PopSelectedItem()
        {
            return RemoveFromItemStack(SelectedSlot, 1);
        }
        
        public ItemStack PeekSelectedItem()
        {
            return ItemStackAt(SelectedSlot);
        }
    }
}