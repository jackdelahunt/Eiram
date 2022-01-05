using System.Collections.Generic;
using Eiram;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Biomes
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/Biome")]
    public class Biome : ScriptableObject
    {
        public BiomeId biomeId;
        public string biomeName;
        public TileId surfaceTile;
        public TileId subSurfaceTile;
        public int surfaceThickness;
        public float terrainHeightProportion;
        public float scale;
        public float treeThreshold;
        public float treeScale;
    }
}