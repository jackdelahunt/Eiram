using Eiram;
using Tiles;
using UnityEditor;
using UnityEngine;

namespace Registers
{
    public class Register : MonoBehaviour
    {
        private static Tile[] tiles;
        [SerializeField] private ConcreteTileData[] concreteTileDataArray;

        public static Tile GetTileById(TileId tileId)
        {
            return tiles[(int)tileId];
        }
        
        public void Awake()
        {
            tiles = new Tile[] {
                new Air(concreteTileDataArray[0]),
                new Dirt(concreteTileDataArray[1]),
            };
            
           if (!Application.isEditor) return;
           // verify every tile and item are indexed correctly
           for (int i = 0; i < tiles.Length; i++)
           {
               Debug.Assert(i == (int)tiles[i].TileId(), $"{i} != {tiles[i].TileId()}");
           }
        }
    }
}