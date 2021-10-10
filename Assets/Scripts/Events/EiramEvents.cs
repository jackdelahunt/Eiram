using System;
using Eiram;
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
    }
}