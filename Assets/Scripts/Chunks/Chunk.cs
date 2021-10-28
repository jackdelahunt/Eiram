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
        private readonly SerialTileData[,] tileDataArray;

        public Chunk(int chunkX)
        {
            this.ChunkX = chunkX;
            tileDataArray = TerrainGenerator.GenerateChunkData(this);
            EiramTilemap.Foreground.DrawChunk(this);
        }
        
        public void Die()
        {
            EiramTilemap.Foreground.RemoveChunk(this);
        }
        
        public SerialTileData GetTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            return tileDataArray[chunkPosition.x, chunkPosition.y];
        }
        
        public void PlaceTileAt(Vector3Int worldPosition, TileId tileId)
        {   
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var data = Register.GetTileByTileId(tileId).DefaultTileData();
            tileDataArray[chunkPosition.x, chunkPosition.y] = data;
            EiramTilemap.Foreground.SetTile(worldPosition, tileId);

            EiramEvents.OnTilePlace(worldPosition, data);
        }

        public void RemoveTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var data = GetTileData(worldPosition);
            
            tileDataArray[chunkPosition.x, chunkPosition.y] = Register.GetTileByTileId(TileId.AIR).DefaultTileData();
            EiramTilemap.Foreground.SetTile(worldPosition, TileId.AIR);
            EiramEvents.OnTileBreak(worldPosition, data);
        }
        
        public void ReplaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            tileDataArray[chunkPosition.x, chunkPosition.y] = Register.GetTileByTileId(tileId).DefaultTileData();
            EiramTilemap.Foreground.SetTile(worldPosition, tileId);
        }
        
        public void UpdateTileAt(Vector3Int worldPosition)
        {
            var data = GetTileData(worldPosition);
            Register.GetTileByTileId(data.TileId).OnUpdate(worldPosition, data);
        }

        public SerialTileData GetTileData(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            return tileDataArray[chunkPosition.x, chunkPosition.y];
        }
        
        public Vector3Int WorldCoordToChunkCoord(Vector3Int worldPosition)
        {
            return new Vector3Int(worldPosition.x - (ChunkX * EiramTypes.CHUNK_WIDTH), worldPosition.y, 0);
        }
    }
}