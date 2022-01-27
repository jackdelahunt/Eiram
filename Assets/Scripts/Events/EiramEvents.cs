using System;
using Eiram;
using Inventories;
using Items;
using Players;
using Tiles;
using UnityEngine;

namespace Events
{
    public static class EiramEvents
    {
        public static event Action<Vector3Int, SerialTileData> TilePlaceEvent;

        public static void OnTilePlace(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            TilePlaceEvent?.Invoke(worldPosition, serialTileData);
        }
        
        public static event Action<Vector3Int, SerialTileData> TileBreakEvent;

        public static void OnTileBreak(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            TileBreakEvent?.Invoke(worldPosition, serialTileData);
        }
        
        public static event Action<PlayerInventory> PlayerTogglePlayerInventoryEvent;

        public static void OnPlayerTogglePlayerInventory(PlayerInventory playerInventory)
        {
            PlayerTogglePlayerInventoryEvent?.Invoke(playerInventory);
        }
        
        public static event Action PlayerInteractEvent;

        public static void OnPlayerInteractEvent()
        {
            PlayerInteractEvent?.Invoke();
        }
        
        public static event Action<int> PlayerChangedHungerEvent;

        public static void OnPlayerChangedHungerEvent(int hunger)
        {
            PlayerChangedHungerEvent?.Invoke(hunger);
        }
        
        public static event Action PlayerInventoryRequestEvent;

        public static void OnPlayerInventoryRequestEvent()
        {
            PlayerInventoryRequestEvent?.Invoke();
        }
        
        public static event Action<ChestInventory> PlayerOpenChestEvent;

        public static void OnPlayerOpenChestEvent(ChestInventory chestInventory)
        {
            PlayerOpenChestEvent?.Invoke(chestInventory);
        }
        
        public static event Action<RichItemStack> ToolBreakEvent;

        public static void OnToolBreakEvent(RichItemStack richItemStack)
        {
            ToolBreakEvent?.Invoke(richItemStack);
        }
        
        public static event Action PlayerToggleNotebookEvent;

        public static void OnPlayerToggleNotebookEvent()
        {
            PlayerToggleNotebookEvent?.Invoke();
        }
        
        public static event Action<int> SelectedSlotChangedEvent;

        public static void OnSelectedSlotChangedEvent(int slotIndex)
        {
            SelectedSlotChangedEvent?.Invoke(slotIndex);
        }
        
        public static event Action SaveToDiskRequestEvent;

        public static void OnSaveToDiskRequestEvent()
        {
            SaveToDiskRequestEvent?.Invoke();
        }
        
        public static event Action<SerialTileData> TileInfoRequestEvent;

        public static void OnTileInfoRequestEvent(SerialTileData tileData)
        {
            TileInfoRequestEvent?.Invoke(tileData);
        }
        
        public static event Action<ItemId> ItemInfoRequestEvent;

        public static void OnItemInfoRequestEvent(ItemId itemId)
        {
            ItemInfoRequestEvent?.Invoke(itemId);
        }
        
        public static event Action<Vector3Int, ItemStack, Player> PlayerAttackEvent;

        public static void OnPlayerAttackEvent(Vector3Int worldPosition, ItemStack inHand, Player player)
        {
            PlayerAttackEvent?.Invoke(worldPosition, inHand, player);
        }
        
    }
}