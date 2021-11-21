using System.Collections.Generic;
using Eiram;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/CropTileData")]
    public class CropTileData : DynamicTileData
    {
        public ItemId SeedItemId;
    }
}