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
            if ((int)tileId < 0 || (int)tileId > (int)TileId.GRASS_FOLIAGE)
            {
                print("");
            }
            return tiles[(int)tileId];
        }
        
        public void Awake()
        {
            tiles = new Tile[] {
                new Air(concreteTileDataArray[0]),
                new Dirt(concreteTileDataArray[1]),
                new Grass(concreteTileDataArray[2]),
                new Stone(concreteTileDataArray[3]),
                null,
                null,
                null,
                null,
                new Bedrock(concreteTileDataArray[8]),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
            };
            
           if (!Application.isEditor) return;
           
           Debug.Assert(tiles.Length == concreteTileDataArray.Length, "Unequal tiles to tile data");

           for (int i = 0; i < tiles.Length; i++)
           {
               if (tiles[i] == null)
               {
                   Debug.LogFormat("Register has null entry at index {0}", i);
                   continue;
               }

               Debug.Assert(i == (int)tiles[i].TileId(), $"{i} != {tiles[i].TileId()}");
           }
        }
    }
}