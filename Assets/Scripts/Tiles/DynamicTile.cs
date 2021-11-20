using System;
using System.Collections.Generic;
using Eiram;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/DynamicTile")]
    public class DynamicTile : ConcreteTileData
    {
        public int maxAge;
        public List<TileBase> tileBases = null;
    }
}