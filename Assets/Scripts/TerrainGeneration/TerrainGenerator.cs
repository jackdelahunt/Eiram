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
            for (int y = 1; y < caveHeight; y++)
            {
                for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
                {
                    var worldPos =
                        Utils.Utils.ChunkPositionToWorldPosition(x, y, chunk.ChunkX);
                    tileIds[x, y] = Noise.CaveNoise(worldPos.x, worldPos.y, 0.4f, 0) ? TileId.STONE : TileId.AIR;
                }
            }
        }

        private static void TerrainPass(TileId[,] tileIds, Chunk chunk, int caveHeight,
            int maxTerrainHeight)
        {
            for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
            {
                var worldPos =
                    Utils.Utils.ChunkPositionToWorldPosition(x, caveHeight, chunk.ChunkX);
                var multiplier = Noise.TerrainNoise(worldPos.x, worldPos.y, 0, 0.1f);
                int actualHeight = Mathf.RoundToInt(maxTerrainHeight * multiplier);
                int highestPoint = (caveHeight + actualHeight) - 1;

                for (int y = caveHeight; y < caveHeight + actualHeight; y++)
                {
                    tileIds[x, y] = y == highestPoint ? TileId.GRASS : TileId.DIRT;
                }
            }
        }
    }
}