using System;
using System.Collections.Generic;
using Eiram;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/ConcreteTileData")]
    public class ConcreteTileData : ScriptableObject
    {
        public string TileName;
        public TileId TileId;
        public ToolType RequiredToolType;
        public ToolLevel RequiredToolLevel;
        public int Hardness;
        public TileBase TileBase;
        public List<LootItem> Drops;
    }

    [Serializable]
    public struct LootItem
    {
        public ItemId ItemId;
        public int Quantity;
        public float Chance;
    }
}