using System.Data;
using System.Linq;
using Biomes;
using Chunks;
using Eiram;
using Registers;
using Tiles;
using UnityEngine;
using Utils;

namespace TerrainGeneration
{
    public class TerrainGenerator
    {
        public static (SerialTileData[,], TileId[,]) GenerateChunkData(Chunk chunk)
        {
            var biome = Register.GetBiomeByBiomeId(chunk.BiomeId);
            var tileDataArray = new SerialTileData[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];
            var backgroundTileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];
            for (int i = 0; i < tileDataArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileDataArray.GetLength(1); j++)
                {
                    tileDataArray[i, j] = Register.GetTileByTileId(TileId.AIR).DefaultTileData();
                    backgroundTileIds[i, j] = TileId.AIR;
                }
            }
            
            // TODO: find out what to do with these hard coded values
            int caveHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.5f);
            int maxTerrainHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * biome.terrainHeightProportion);

            BedrockPass(tileDataArray, backgroundTileIds);
            CavePass(tileDataArray, backgroundTileIds, chunk, caveHeight);
            TerrainPass(tileDataArray, backgroundTileIds, chunk, biome, caveHeight, maxTerrainHeight);
    
            return (tileDataArray, backgroundTileIds);
        }

        private static void BedrockPass(SerialTileData[,] tileDataArray, TileId[,] backgroundTileId)
        {
            for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
            {
                SetTile(x, 0, TileId.BEDROCK, tileDataArray);
                backgroundTileId[x, 0] = TileId.BEDROCK;
            }
        }
        private static void CavePass(SerialTileData[,] tileDataArray, TileId[,] backgroundTileIds, Chunk chunk, int caveHeight)
        {
            int xOffset = 0;
            int yOffset = 1;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, yOffset, chunk.ChunkX);
            
            while (yOffset < caveHeight)
            {
                while (xOffset < EiramTypes.CHUNK_WIDTH)
                {
                    var tile =
                        Noise.CaveNoise(firstTileWorldPos.x + xOffset,
                            firstTileWorldPos.y + yOffset, 0.45f, chunk.Seed, scale: 0.2f)
                            ? TileId.STONE
                            : TileId.AIR;
                    
                    foreach (var lode in Register.lodes)
                    {
                        if(firstTileWorldPos.y + yOffset < lode.MinHeight || firstTileWorldPos.y + yOffset > lode.MaxHeight) continue;
                        
                        if (Noise.CaveNoise(firstTileWorldPos.x + xOffset,
                            firstTileWorldPos.y + yOffset, lode.Threshold, chunk.Seed, lode.Scale))
                        {   
                            tile = lode.TileId;
                            break;
                        }    
                    }

                    SetTile(xOffset, yOffset, tile, tileDataArray);
                    backgroundTileIds[xOffset, yOffset] = TileId.STONE;
                    xOffset++;
                }

                yOffset++;
                xOffset = 0;
            }
        }

        private static void TerrainPass(SerialTileData[,] tileDataArray, TileId[,] backgroundTileIds, Chunk chunk, Biome biome, int caveHeight,
            int maxTerrainHeight)
        {
            int xOffset = 0;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, caveHeight, chunk.ChunkX);
            
            while (xOffset < EiramTypes.CHUNK_WIDTH)
            {
                var multiplier = Noise.TerrainNoise(firstTileWorldPos.x + xOffset, firstTileWorldPos.y, chunk.Seed, biome.scale);
                int actualHeight = Mathf.RoundToInt(maxTerrainHeight * multiplier);
                int highestPoint = (caveHeight + actualHeight) - 1;

                for (int y = highestPoint; y >= caveHeight; y--)
                {
                    var tile = y > highestPoint - biome.surfaceThickness
                        ? biome.surfaceTile
                        : biome.subSurfaceTile;
                    
                    SetTile(xOffset, y, tile, tileDataArray);
                    backgroundTileIds[xOffset, y] = tile;
                }
                
                // plant some trees
                if (Noise.CaveNoise(firstTileWorldPos.x + xOffset, highestPoint + 1, biome.treeThreshold, chunk.Seed, biome.treeScale))
                {
                    MakeTree(tileDataArray, biome, xOffset, highestPoint + 1);
                }
                else // plant some foliage
                {
                    foreach (var foliage in biome.foliageList)
                    {
                        if (Noise.CaveNoise(firstTileWorldPos.x + xOffset, highestPoint + 1, foliage.threshold, chunk.Seed,
                            foliage.scale))
                        {
                            SetTile(xOffset, highestPoint + 1, foliage.tileID, tileDataArray);
                        }
                    }
                }

                xOffset++;
            }
        }
        
        private static void MakeTree(SerialTileData[,] tileDataArray, Biome biome, int x, int y)
        {
            SetTile(x, y, TileId.LOG, tileDataArray);
            SetTile(x, y + 1, TileId.LOG, tileDataArray);
            SetTile(x, y + 2, TileId.LOG, tileDataArray);
            SetTile(x, y + 3, TileId.LOG, tileDataArray);
            SetTile(x, y + 4, TileId.LOG, tileDataArray);
            SetTile(x, y + 5, TileId.LOG, tileDataArray);

            SetTile(x - 1, y + 4, TileId.LEAVES, tileDataArray);
            SetTile(x - 1, y + 5, TileId.LEAVES, tileDataArray);
            SetTile(x + 1, y + 4, TileId.LEAVES, tileDataArray);
            SetTile(x + 1, y + 5, TileId.LEAVES, tileDataArray);
            SetTile(x, y + 6, TileId.LEAVES, tileDataArray);
        }

        private static void SetTile(int x, int y, TileId id, SerialTileData[,] tileDataArray)
        {
            if (x >= 0 && x < EiramTypes.CHUNK_WIDTH)
            {
                if (y >= 0 && y < EiramTypes.CHUNK_HEIGHT)
                {
                    tileDataArray[x, y] = Register.GetTileByTileId(id).DefaultTileData();
                }
            }
        }
    }
}