using System;
using Eiram;
using Items.Items;
using JetBrains.Annotations;
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

        public static AbstractTile GetTileByTileId(TileId tileId)
        {
            return tiles[(int)tileId];
        }

        [CanBeNull]
        public static AbstractTile GetTileByItemId(ItemId itemId)
        {
            Debug.Assert(itemId != ItemId.UNKNOWN);
            
            foreach (var tile in tiles)
            {
                if (tile.ItemId() == itemId)
                    return tile;
            }

            return null;
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
                new TilledSoil(concreteTileDataArray[17]),
            };

            items = itemArray;

#if UNITY_EDITOR
            if (tiles.Length != concreteTileDataArray.Length)
               throw new Exception("Unequal tiles to tile data");

            for (int i = 0; i < tiles.Length; i++)
            {
                if(tiles[i] == null) continue;
                
                if (i != (int) tiles[i].TileId())
                    throw new Exception($"TileId is in the wrong slot index is {i}");
            }
           
            for (int i = 0; i < items.Length; i++)
            {
                if(items[i] == null) continue;

                if (i != (int) items[i].itemId)
                    throw new Exception($"ItemId is in the wrong slot index is {i}");
            }
#endif
        }
    }
}