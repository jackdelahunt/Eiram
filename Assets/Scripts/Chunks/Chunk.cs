using System.Linq;
using Eiram;
using Registers;
using Tilemaps;
using UnityEngine;
using Utils;

namespace Chunks
{
    public class Chunk
    {
        public int ChunkX;
        private TileId[,] tileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];

        public Chunk()
        {
            ChunkX = 0;
            for (int i = 0; i < tileIds.GetLength(0); i++)
            {
                for (int j = 0; j < tileIds.GetLength(1); j++)
                {
                    tileIds[i, j] = TileId.DIRT;
                }
            }
            
            EiramTilemap.Instance.DrawChunk(this);
        }

        public TileId GetTileAt(Vector3Int worldPosition)
        {
            var chunkPos = Convert.WorldPositionToChunkPosition(worldPosition);
            Debug.Assert(chunkPos.x >= 0 && chunkPos.x < EiramTypes.CHUNK_WIDTH, "Converted X result is out of bounds");
            Debug.Assert(chunkPos.y>= 0 && chunkPos.x < EiramTypes.CHUNK_HEIGHT, "Converted Y result is out of bounds");
            return tileIds[chunkPos.x, chunkPos.y];
        }
    }
}