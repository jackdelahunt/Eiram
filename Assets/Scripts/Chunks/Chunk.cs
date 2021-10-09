using System.Linq;
using Eiram;
using Registers;
using Tilemaps;
using UnityEngine;
using Utils;
using TerrainGeneration;

namespace Chunks
{
    public class Chunk
    {
        public int ChunkX;
        private TileId[,] tileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];

        public Chunk(int chunkX)
        {
            this.ChunkX = chunkX;
            tileIds = TerrainGenerator.GenerateChunkData(this);
            
            EiramTilemap.Instance.DrawChunk(this);
        }

        public void Die()
        {
            EiramTilemap.Instance.RemoveChunk(this);
        }
        
        public TileId GetTileAt(Vector3Int worldPosition)
        {
            var chunkPos = WorldCoordToChunkCoord(worldPosition);
            return tileIds[chunkPos.x, chunkPos.y];
        }
        
        public Vector3Int WorldCoordToChunkCoord(Vector3Int worldPosition)
        {
            return new Vector3Int(worldPosition.x - (ChunkX * EiramTypes.CHUNK_WIDTH), worldPosition.y, 0);
        }
    }
}