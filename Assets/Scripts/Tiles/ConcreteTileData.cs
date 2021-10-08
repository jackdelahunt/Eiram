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
        public TileBase TileBase;
    }
}