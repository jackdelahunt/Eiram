using System;
using Inventories;

namespace Tiles
{
    [Serializable]
    public class SerialFatTileData
    {
        public int X;
        public int Y;
    }

    [Serializable]
    public class SerialChestTileData : SerialFatTileData
    {
        public ChestInventory ChestInventory;
    }
}