using System;
using Eiram;
using Inventories;
using UnityEngine;

namespace Events
{
    public static class EiramEvents
    {
        public static event Action<Vector3Int, TileId> TileBreakEvent;

        public static void OnTileBreak(Vector3Int worldPosition, TileId tileId)
        {
            TileBreakEvent?.Invoke(worldPosition, tileId);
        }
        
        public static event Action<PlayerInventory> PlayerToggleInventoryEvent;

        public static void OnPlayerToggleInventory(PlayerInventory playerInventory)
        {
            PlayerToggleInventoryEvent?.Invoke(playerInventory);
        }
        
        public static event Action<PlayerInventory> PlayerInventoryIsDirtyEvent;

        public static void OnPlayerInventoryIsDirty(PlayerInventory playerInventory)
        {
            PlayerInventoryIsDirtyEvent?.Invoke(playerInventory);
        }
    }
}