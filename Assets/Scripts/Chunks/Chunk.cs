using Eiram;
using Events;
using Registers;
using Tilemaps;
using UnityEngine;
using TerrainGeneration;
using Tiles;

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
            tileIds[chunkPosition.x, chunkPosition.y] = tileId;
            EiramTilemap.Foreground.SetTile(worldPosition, tileId);
        }

        public void RemoveTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var tileIdAt = tileIds[chunkPosition.x, chunkPosition.y];
            tileIds[chunkPosition.x, chunkPosition.y] = TileId.AIR;
            EiramTilemap.Foreground.SetTile(worldPosition, TileId.AIR);
            EiramEvents.OnTileBreak(worldPosition, tileIdAt);
        }
        
        public void ReplaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            tileIds[chunkPosition.x, chunkPosition.y] = tileId;
            EiramTilemap.Foreground.SetTile(worldPosition, tileId);
        }
        
        public void UpdateTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var tileId = tileIds[chunkPosition.x, chunkPosition.y];
            Register.GetTileByTileId(tileId).OnUpdate(worldPosition, GetTileData(worldPosition));
        }

        public SerialTileData GetTileData(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var tileIdAt = tileIds[chunkPosition.x, chunkPosition.y];
            return Register.GetTileByTileId(tileIdAt).DefaultTileData();
        }
        
        public Vector3Int WorldCoordToChunkCoord(Vector3Int worldPosition)
        {
            return new Vector3Int(worldPosition.x - (ChunkX * EiramTypes.CHUNK_WIDTH), worldPosition.y, 0);
        }
    }
}