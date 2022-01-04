using System;
using Biomes;
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
        private static Biome[] biomes;
        
        [SerializeField] private ConcreteTileData[] concreteTileDataArray;
        [SerializeField] private Item[] itemArray;
        [SerializeField] private Biome[] biomeArray;

        public static AbstractTile GetTileByTileId(TileId tileId)
        {
            return tiles[(int)tileId];
        }
        
        public static Item GetItemByItemId(ItemId itemId)
        {
            return items[(int)itemId];
        }

        public static Biome GetBiomeByBiomeId(BiomeId biomeId)
        {
            return biomes[(int)biomeId];
        }

        public static int ActiveTiles() => tiles.Length;
        public static int ActiveItems() => items.Length;
        public static int ActiveBiomes() => biomes.Length;
        
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
                new Thorns(concreteTileDataArray[18]),
                new Trellis(concreteTileDataArray[19]),
            };

            items = itemArray;
            biomes = biomeArray;

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
            
            for (int i = 0; i < biomes.Length; i++)
            {
                if(biomes[i] == null) continue;

                if (i != (int) biomes[i].biomeId)
                    throw new Exception($"BiomeId is in the wrong slot index is {i}");
            }
#endif
        }
    }
}