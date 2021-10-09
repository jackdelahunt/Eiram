using System.Linq;
using Chunks;
using Eiram;
using Registers;
using UnityEngine;

namespace TerrainGeneration
{
    public class TerrainGenerator
    {
        public static TileId[,] GenerateChunkData(Chunk chunk)
        {
            var tileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];
            
            // TODO: find out what to do with these hard coded values
            int caveHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.5f);
            int terrainHeight = Mathf.RoundToInt(EiramTypes.CHUNK_HEIGHT * 0.08f);

            BedrockPass(tileIds);
            CavePass(tileIds, chunk, caveHeight);
            TerrainPass(tileIds, chunk, caveHeight, terrainHeight);

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
                    tileIds[x, y] = TileId.STONE;
                }
            }
        }

        private static void TerrainPass(TileId[,] tileIds, Chunk chunk, int caveHeight,
            int terrainHeight)
        {
            for (int y = caveHeight; y < caveHeight + terrainHeight; y++)
            {
                for (int x = 0; x < EiramTypes.CHUNK_WIDTH; x++)
                {
                    tileIds[x, y] = TileId.GRASS;
                }
            }
        }
    }
}