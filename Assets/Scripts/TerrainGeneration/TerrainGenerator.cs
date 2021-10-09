using System.Data;
using System.Linq;
using Chunks;
using Eiram;
using Registers;
using UnityEngine;
using Utils;

namespace TerrainGeneration
{
    public class TerrainGenerator
    {
        public static TileId[,] GenerateChunkData(Chunk chunk)
        {
            var tileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];
            
            // TODO: find out what to do with these hard coded values
            int caveHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.5f);
            int maxTerrainHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.1f);

            BedrockPass(tileIds);
            CavePass(tileIds, chunk, caveHeight);
            TerrainPass(tileIds, chunk, caveHeight, maxTerrainHeight);

            return tileIds;
        }

        private static void BedrockPass(TileId[,] tileIds)
        {
            for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
            {
                tileIds[x, 0] = TileId.BEDROCK;
            }
        }

        private static void CavePass(TileId[,] tileIds, Chunk chunk, int caveHeight)
        {
            int xOffset = 0;
            int yOffset = 1;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, yOffset, chunk.ChunkX);
            
            while (yOffset < caveHeight)
            {
                while (xOffset < EiramTypes.CHUNK_WIDTH)
                {
                    tileIds[xOffset, yOffset] = Noise.CaveNoise(firstTileWorldPos.x + xOffset, firstTileWorldPos.y + yOffset, 0.4f, 0) ? TileId.STONE : TileId.AIR;
                    xOffset++;
                }

                yOffset++;
                xOffset = 0;
            }
        }

        private static void TerrainPass(TileId[,] tileIds, Chunk chunk, int caveHeight,
            int maxTerrainHeight)
        {
            int xOffset = 0;
            var firstTileWorldPos =
                Utils.Utils.ChunkPositionToWorldPosition(xOffset, caveHeight, chunk.ChunkX);
            
            while (xOffset < EiramTypes.CHUNK_WIDTH)
            {
                var multiplier = Noise.TerrainNoise(firstTileWorldPos.x + xOffset, firstTileWorldPos.y, 0, 0.1f);
                int actualHeight = Mathf.RoundToInt(maxTerrainHeight * multiplier);
                int highestPoint = (caveHeight + actualHeight) - 1;

                for (int y = caveHeight; y < caveHeight + actualHeight; y++)
                {
                    // xOffset is equal to the local x of the tile aswell
                    tileIds[xOffset, y] = y == highestPoint ? TileId.GRASS : TileId.DIRT;
                }

                xOffset++;
            }
        }
    }
}