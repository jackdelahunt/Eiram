using System;
using Eiram;
using Events;
using IO;
using Registers;
using Tilemaps;
using UnityEngine;
using TerrainGeneration;
using Tiles;
using Worlds;

namespace Chunks
{
    [Serializable]
    public class Chunk
    {
        public readonly int ChunkX;
        private readonly SerialTileData[,] tileDataArray;

        public Chunk(int chunkX)
        {
            this.ChunkX = chunkX;
            if (World.Current.Save.Region.GetFile($"{ChunkX}.chunk").IsSome(out var file))
            {
                var loadResult = Filesystem.LoadFrom<Chunk>($"{ChunkX}.chunk", World.Current.Save.Region);
                if (loadResult.IsNone())
                {
                    // delete bad data
                    file.Delete();
                    tileDataArray = TerrainGenerator.GenerateChunkData(this);
                }
                else
                {
                    tileDataArray = loadResult.Value.tileDataArray;
                }
            }
            else
            {
                tileDataArray = TerrainGenerator.GenerateChunkData(this);
            }
            EiramTilemap.Foreground.DrawChunk(this);
        }
        
        public void Die()
        {
            EiramTilemap.Foreground.RemoveChunk(this);
            Filesystem.SaveTo(this, $"{ChunkX}.chunk", World.Current.Save.Region);
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