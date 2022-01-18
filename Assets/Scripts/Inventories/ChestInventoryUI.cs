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
            EiramEvents.OnPlayerInventoryRequestEvent();
            activeInventory = Some(chestInventory as Inventory);
            OpenInventory();
        }

        private void OnPlayerInteractEvent()
        {
            if (!toggled) return;
            activeInventory = None<Inventory>();
            CloseInventory();
        }
    }
}