using Eiram;
using Tilemaps;
using UnityEngine;
using TerrainGeneration;

namespace Chunks
{
    public class Chunk
    {
        public readonly int ChunkX;
        private readonly TileId[,] tileIds;

        public Chunk(int chunkX)
        {
            this.ChunkX = chunkX;
            tileIds = TerrainGenerator.GenerateChunkData(this);
            
            EiramTilemap.Foreground.DrawChunk(this);
        }

        public void Die()
        {
            EiramTilemap.Foreground.RemoveChunk(this);
        }
        
        public TileId GetTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            return tileIds[chunkPosition.x, chunkPosition.y];
        }
        
        public void PlaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);

            if (tileIds[chunkPosition.x, chunkPosition.y] == TileId.AIR)
            {
                EiramTilemap.Foreground.SetTile(worldPosition, tileId);
            }
        }

        public void RemoveTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            tileIds[chunkPosition.x, chunkPosition.y] = TileId.AIR;
            EiramTilemap.Foreground.SetTile(worldPosition, TileId.AIR);
        }
        
        public Vector3Int WorldCoordToChunkCoord(Vector3Int worldPosition)
        {
            return new Vector3Int(worldPosition.x - (ChunkX * EiramTypes.CHUNK_WIDTH), worldPosition.y, 0);
        }
    }
}