using System;
using Events;
using Players;
using static Eiram.Handles;
using UnityEngine;

namespace Inventories
{
    public class ChestInventoryUI : InventoryUI
    {
        protected override void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
            EiramEvents.PlayerOpenChestEvent += OnPlayerOpenChestEvent;
            AwakeInventoryUI();
        }

        private new void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
            EiramEvents.PlayerOpenChestEvent -= OnPlayerOpenChestEvent;
        }

        private void OnPlayerOpenChestEvent(ChestInventory chestInventory)
        {
            if(Player.InInventory) return;
            Player.InInventory = true;
            OpenInventory();
            activeInventory = Some(chestInventory as Inventory);
            toggled = true;
        }

        private void OnPlayerToggleInventoryEvent()
        {
            if (!toggled) return;
            Player.InInventory = false;
            CloseInventory();
            activeInventory = None<Inventory>();
            toggled = false;
        }
    }
}