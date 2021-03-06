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
                new Cobblestone(concreteTileDataArray[6]),
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
                new MiniTree(concreteTileDataArray[23]),
                new StonePlant(concreteTileDataArray[24]),
                new CranberryBush(concreteTileDataArray[25]),
                new Gold(concreteTileDataArray[26]),
                new CopperPlant(concreteTileDataArray[27]),
                new GoldPlant(concreteTileDataArray[28]),
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
                new OrganicMassItem(itemArray[13]),
                new MiniTreeItem(itemArray[14]),
                new CobblestoneItem(itemArray[15]),
                new StoneItem(itemArray[16]),
                new StoneSeedItem(itemArray[17]),
                new StoneShovelItem(itemArray[18]),
                new StonePickaxeItem(itemArray[19]),
                new StoneAxeItem(itemArray[20]),
                new CopperItem(itemArray[21]),
                new CopperSeedItem(itemArray[22]),
                new CopperShovelItem(itemArray[23]),
                new CopperPickaxeItem(itemArray[24]),
                new CopperAxeItem(itemArray[25]),
                new GoldItem(itemArray[26]),
                new GoldSeedItem(itemArray[27]),
                new GoldShovelItem(itemArray[28]),
                new GoldPickaxeItem(itemArray[29]),
                new GoldAxeItem(itemArray[30]),
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