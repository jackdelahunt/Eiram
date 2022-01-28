using System;
using Biomes;
using Chunks;
using Eiram;
using Items;
using Items.Items;
using JetBrains.Annotations;
using Recipes;
using Tiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using static Eiram.Handles;

namespace Registers
{
    public class Register : MonoBehaviour
    {
        public static AbstractTile[] tiles;
        public static AbstractItem[] items;
        public static Biome[] biomes;
        public static CropRecipe[] cropRecipes;
        public static BuildingRecipe[] buildingRecipes;
        public static Lode[] lodes;
        
        [SerializeField] private ConcreteTileData[] concreteTileDataArray;
        [SerializeField] private ConcreteItemData[] itemArray;
        [SerializeField] private Biome[] biomeArray;
        [SerializeField] private CropRecipe[] cropRecipeArray;
        [SerializeField] private BuildingRecipe[] buildingRecipeArray;
        [SerializeField] private Lode[] lodeArray;

        public static AbstractTile GetTileByTileId(TileId tileId)
        {
            return tiles[(int)tileId];
        }
        
        public static AbstractItem GetItemByItemId(ItemId itemId)
        {
            return items[(int)itemId];
        }

        public static Biome GetBiomeByBiomeId(BiomeId biomeId)
        {
            return biomes[(int)biomeId];
        }

        public static Option<CropRecipe> GetCropRecipe(TileId left, TileId right)
        {
            foreach (var recipe in cropRecipes)
            {
                if (recipe.LeftCrop == left && recipe.RightCrop == right) return recipe;

                if (!recipe.Strict)
                {
                    if (recipe.RightCrop == left && recipe.LeftCrop == right) return recipe;
                }
            }

            return None<CropRecipe>();
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
                new Log(concreteTileDataArray[9]),
                new Wood(concreteTileDataArray[10]),
                new Plank(concreteTileDataArray[11]),
                new Leaves(concreteTileDataArray[12]),
                null,
                null,
                null,
                null,
                new TilledSoil(concreteTileDataArray[17]),
                new Thorns(concreteTileDataArray[18]),
                new Trellis(concreteTileDataArray[19]),
                new Chest(concreteTileDataArray[20]),
                new Copper(concreteTileDataArray[21]),
                new Scrap(concreteTileDataArray[22]),
            };

            items = new AbstractItem[]
            {
                new DirtItem(itemArray[0]),
                new GrassItem(itemArray[1]),
                new ThornsItem(itemArray[2]),
                new TrellisItem(itemArray[3]),
                new ChestItem(itemArray[4]),
                new WoodShovelItem(itemArray[5]),
                new WoodPickaxeItem(itemArray[6]),
                new WoodAxeItem(itemArray[7]),
                new CranberriesItem(itemArray[8]),
                new StickItem(itemArray[9]),
                new WoodClumpItem(itemArray[10]),
                new WoodItem(itemArray[11]),
                new PlankItem(itemArray[12]),
            };
            
            biomes = biomeArray;
            cropRecipes = cropRecipeArray;
            buildingRecipes = buildingRecipeArray;
            lodes = lodeArray;

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

                if (i != (int) items[i].ItemId())
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