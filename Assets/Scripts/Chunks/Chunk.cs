using System;
using System.Collections.Generic;
using Eiram;
using Events;
using IO;
using Players;
using Registers;
using Tilemaps;
using UnityEngine;
using TerrainGeneration;
using Tiles;
using Utils;
using Worlds;
using static Eiram.Handles;
using Random = System.Random;

namespace Chunks
{
    [Serializable]
    public class Chunk
    {
        // time = growthStages / delta / (RANDOM_TILE_UPDATE_COUNT / WIDTH * HEIGHT)
        private const int RANDOM_TILE_UPDATE_COUNT = 2000;
        public readonly BiomeId BiomeId;
        public readonly int ChunkX;
        private readonly SerialTileData[,] tileDataArray;
        public readonly TileId[,] backgroundTileData;
        private readonly List<SerialFatTileData> fatTileArray;

        public Chunk(int chunkX)
        {
            var rand = new Random();
            this.BiomeId = (BiomeId)Mathf.Round(Noise.TerrainNoise(chunkX, 0, 0) * (float)(Register.ActiveBiomes() - 1));
            this.ChunkX = chunkX;
            (tileDataArray, backgroundTileData) = TerrainGenerator.GenerateChunkData(this);
            fatTileArray = new List<SerialFatTileData>();
            EiramTilemap.Foreground.DrawChunk(this);
            EiramTilemap.Background.DrawChunk(this);
        }

        public Chunk(ChunkData loadedData)
        {
            this.ChunkX = loadedData.ChunkX;
            this.tileDataArray = loadedData.TileDataArray;
            this.backgroundTileData = loadedData.BackgroundTileData;
            this.fatTileArray = loadedData.FatTileArray;
            EiramTilemap.Foreground.DrawChunk(this);
            EiramTilemap.Background.DrawChunk(this);
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
        
        public TileId GetBackgroundTileAt(Vector3Int worldPosition)
        {
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            return backgroundTileData[chunkPosition.x, chunkPosition.y];
        }
        
        public bool PlaceTileAt(Vector3Int worldPosition, TileId tileId)
        {   
            var chunkPosition = WorldCoordToChunkCoord(worldPosition);
            var tile = Register.GetTileByTileId(tileId);
            var data = tile.DefaultTileData();

            if (tileDataArray[chunkPosition.x, chunkPosition.y].TileId != TileId.AIR)
                return false;
            
            if (tile.CanPlace(worldPosition, data))
            {
                tileDataArray[chunkPosition.x, chunkPosition.y] = data;
                EiramTilemap.Foreground.SetTile(worldPosition, data);

                EiramEvents.OnTilePlace(worldPosition, data);
                return true;
            }

            return false;
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
        
        public bool UseTileAt(Vector3Int worldPosition, Player player)
        {
            var data = GetTileData(worldPosition);
            return Register.GetTileByTileId(data.TileId).OnUse(worldPosition, data, player);
        }

        public void RandomUpdateTileAt(Vector3Int worldPosition)
        {
            var data = GetTileData(worldPosition);
            Register.GetTileByTileId(data.TileId).OnRandomUpdate(worldPosition, data);
        }

        public void RandomUpdate()
        {
            var rand = new Random();
            for(int i = 0; i < RANDOM_TILE_UPDATE_COUNT; i++)
            {
                int randomX = (int) (rand.NextDouble() * EiramTypes.CHUNK_WIDTH);
                int randomY = (int) (rand.NextDouble() * EiramTypes.CHUNK_HEIGHT);

                var worldPosition = ChunkCoordToWorldCoord(new Vector3Int(randomX, randomY, 0));
                RandomUpdateTileAt(worldPosition);  
            }
        }

        public void AddFatTile(SerialFatTileData serialFatTileData)
        {
            fatTileArray.Add(serialFatTileData);
        }

        public void RemoveFatTile(Vector3Int worldPosition)
        {
            SerialFatTileData found = null;
            foreach (var fatTile in fatTileArray)
            {
                if (fatTile.Y == worldPosition.y && fatTile.X == worldPosition.x)
                {
                    found = fatTile;
                    break;
                }
            }

            fatTileArray.Remove(found);
        }

        public Option<SerialFatTileData> GetFatTileAt(Vector3Int worldPosition)
        {
            foreach (var tileEntity in fatTileArray)
            {
                if (tileEntity.Y == worldPosition.y && tileEntity.X == worldPosition.x)
                    return tileEntity;
            }

            return None<SerialFatTileData>();
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
        
        public Vector3Int ChunkCoordToWorldCoord(Vector3Int chunkPosition)
        {
            return new Vector3Int(chunkPosition.x + (EiramTypes.CHUNK_WIDTH * ChunkX), chunkPosition.y, 0);
        }

        public ChunkData SerializableData()
        {
            return new ChunkData
            {
                ChunkX = this.ChunkX,
                TileDataArray = this.tileDataArray,
                BackgroundTileData = this.backgroundTileData,
                FatTileArray = this.fatTileArray
            };
        }
    }

    [Serializable]
    public class ChunkData
    {
        public int ChunkX;
        public SerialTileData[,] TileDataArray;
        public TileId[,] BackgroundTileData;
        public List<SerialFatTileData> FatTileArray;
    }
}