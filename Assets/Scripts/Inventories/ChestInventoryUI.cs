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
            EiramEvents.PlayerOpenChestEvent += OnPlayerOpenChestEvent;
            EiramEvents.PlayerInteractEvent += OnPlayerInteractEvent;
            AwakeInventoryUI();
        }

        private new void OnDestroy()
        {
            EiramEvents.PlayerOpenChestEvent -= OnPlayerOpenChestEvent;
            EiramEvents.PlayerInteractEvent -= OnPlayerInteractEvent;
        }

        private void OnPlayerOpenChestEvent(ChestInventory chestInventory)
        {
            OpenInventory();
            EiramEvents.OnPlayerInventoryRequestEvent();
            activeInventory = Some(chestInventory as Inventory);
            toggled = true;
        }

        private void OnPlayerInteractEvent()
        {
            if (!toggled) return;
            CloseInventory();
            activeInventory = None<Inventory>();
            toggled = false;
        }
    }
}