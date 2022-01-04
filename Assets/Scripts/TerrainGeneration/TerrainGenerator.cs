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
        public static SerialTileData[,] GenerateChunkData(Chunk chunk)
        {
            var biome = Register.GetBiomeByBiomeId(chunk.BiomeId);
            var tileDataArray = new SerialTileData[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];
            for (int i = 0; i < tileDataArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileDataArray.GetLength(1); j++)
                {
                    tileDataArray[i, j] = Register.GetTileByTileId(TileId.AIR).DefaultTileData();
                }
            }
            
            // TODO: find out what to do with these hard coded values
            int caveHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.5f);
            int maxTerrainHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * biome.terrainHeightProportion);

            BedrockPass(tileDataArray);
            CavePass(tileDataArray, chunk, caveHeight);
            TerrainPass(tileDataArray, chunk, biome, caveHeight, maxTerrainHeight);
    
            return tileDataArray;
        }

        private static void BedrockPass(SerialTileData[,] tileDataArray)
        {
            for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
            {
                SetTile(x, 0, TileId.BEDROCK, tileDataArray);
            }
        }
        private static void CavePass(SerialTileData[,] tileDataArray, Chunk chunk, int caveHeight)
        {
            int xOffset = 0;
            int yOffset = 1;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, yOffset, chunk.ChunkX);
            
            while (yOffset < caveHeight)
            {
                while (xOffset < EiramTypes.CHUNK_WIDTH)
                {
                    SetTile(xOffset, yOffset, Noise.CaveNoise(firstTileWorldPos.x + xOffset, firstTileWorldPos.y + yOffset, 0.4f, 0) ? TileId.STONE : TileId.AIR, tileDataArray);
                    xOffset++;
                }

                yOffset++;
                xOffset = 0;
            }
        }

        private static void TerrainPass(SerialTileData[,] tileDataArray, Chunk chunk, Biome biome, int caveHeight,
            int maxTerrainHeight)
        {
            int xOffset = 0;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, caveHeight, chunk.ChunkX);
            
            while (xOffset < EiramTypes.CHUNK_WIDTH)
            {
                var multiplier = Noise.TerrainNoise(firstTileWorldPos.x + xOffset, firstTileWorldPos.y, 0, biome.scale);
                int actualHeight = Mathf.RoundToInt(maxTerrainHeight * multiplier);
                int highestPoint = (caveHeight + actualHeight) - 1;

                for (int y = highestPoint; y >= caveHeight; y--)
                {
                    SetTile(xOffset, y, y > highestPoint - biome.surfaceThickness ? biome.surfaceTile : biome.subSurfaceTile, tileDataArray);
                }
                
                // plant some trees
                if (Noise.CaveNoise(xOffset, highestPoint + 1, biome.treeThreshold, 0, biome.treeScale))
                {
                    MakeTree(tileDataArray, biome, xOffset, highestPoint + 1);
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