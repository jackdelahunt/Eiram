using System;
using Eiram;
using Inventories;
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
        
        public static event Action<PlayerInventory> PlayerToggleInventoryEvent;

        public static void OnPlayerToggleInventory(PlayerInventory playerInventory)
        {
            PlayerToggleInventoryEvent?.Invoke(playerInventory);
        }
        
        public static event Action<int> SelectedSlotChangedEvent;

        public static void SelectedSlotChanged(int slotIndex)
        {
            SelectedSlotChangedEvent?.Invoke(slotIndex);
        }
        
    }
}