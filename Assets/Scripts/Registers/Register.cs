using Eiram;
using Items.Items;
using Tiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Registers
{
    public class Register : MonoBehaviour
    {
        private static AbstractTile[] tiles;
        private static Item[] items;
        
        [SerializeField] private ConcreteTileData[] concreteTileDataArray;
        [SerializeField] private Item[] itemArray;

        public static AbstractTile GetTileById(TileId tileId)
        {
            return tiles[(int)tileId];
        }
        
        public static Item GetItemById(ItemId itemId)
        {
            return items[(int)itemId];
        }
        
        public void Awake()
        {
            tiles = new AbstractTile[] {
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

            items = itemArray;
            
           if (!Application.isEditor) return;
           
           Debug.Assert(tiles.Length == concreteTileDataArray.Length, "Unequal tiles to tile data");

           for (int i = 0; i < tiles.Length; i++)
           {
               if (tiles[i] == null)
               {
                   Debug.LogFormat("Register has tile of null entry at index {0}", i);
                   continue;
               }

               Debug.Assert(i == (int)tiles[i].TileId(), $"{i} != {tiles[i].TileId()}");
           }
           
           Debug.Assert(items.Length == itemArray.Length, "Unequal items to item array");

           for (int i = 0; i < items.Length; i++)
           {
               if (items[i] == null)
               {
                   Debug.LogFormat("Register has  item null entry at index {0}", i);
                   continue;
               }

               Debug.Assert(i == (int)(items[i].itemId), $"{i} != {items[i].itemId}");
           }
        }
    }
}