using System;
using System.Collections.Generic;
using Chunks;
using Eiram;
using Events;
using IO;
using Items;
using Players;
using Registers;
using Tiles;
using UnityEngine;
using static Eiram.Handles;

namespace Worlds
{
    public class World : MonoBehaviour
    {
        public static World Current = null;

        [SerializeField] private GameObject itemEntityPrefab = null;
        
        private GameObject playerObject;
        private Player player;
        private Dictionary<int, Chunk> activeChunks = new Dictionary<int, Chunk>();
        public Save Save;

        private void Awake()
        {
            EiramEvents.TilePlaceEvent += OnTilePlace;
            EiramEvents.TileBreakEvent += OnTileBreak;
            EiramEvents.SaveToDiskRequestEvent += SaveAllData;
            Current = this;
            playerObject = GameObject.FindGameObjectWithTag("Player");
            player = playerObject.GetComponent<Player>();
            Save = Filesystem.CreateSave("DEBUG_SAVE");
        }

        void Start()
        {
            InvokeRepeating(nameof(ChunkRefresh), 0.0f, 1.0f);
            InvokeRepeating(nameof(RandomUpdateChunks), 0.0f, 1.0f);
        }   

        private void OnDestroy()
        {
            EiramEvents.TilePlaceEvent -= OnTilePlace;
            EiramEvents.TileBreakEvent -= OnTileBreak;
            EiramEvents.SaveToDiskRequestEvent -= SaveAllData;
        }
        
        public bool PlaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                return chunk.PlaceTileAt(worldPosition, tileId);
            }

            return false;
        }

        public void RemoveTileAtAsPlayer(Vector3Int worldPosition, ItemStack inHand, Player player)
        {
            if (GetTileData(worldPosition).IsSome(out var tileData))
            {
                if (inHand.ItemId != ItemId.UNKNOWN &&  Register.GetItemByItemId(inHand.ItemId).IsToolItem(out var _, out var toolItem))
                {
                    Register.GetItemByItemId(toolItem.itemId).OnBreak(worldPosition, inHand, player);
                }
                
                RemoveTileAt(worldPosition);
            }
        }

        public void RemoveTileAt(Vector3Int worldPosition)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.RemoveTileAt(worldPosition);
            }
        }

        public void ReplaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.ReplaceTileAt(worldPosition, tileId);
            }
        }
        
        public void UpdateTileAt(Vector3Int worldPosition)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.UpdateTileAt(worldPosition);
            }
        }
        
        public bool UseTileAt(Vector3Int worldPosition, Player player)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                return chunk.UseTileAt(worldPosition, player);
            }

            return false;
        }
        
        public void RandomUpdateTileAt(Vector3Int worldPosition)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.RandomUpdateTileAt(worldPosition);
            }
        }

        public void SaveWorld()
        {
            EiramEvents.OnSaveToDiskRequestEvent();
        }

        public void AddFatTile(Vector3Int worldPosition, SerialFatTileData serialFatTileData)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.AddFatTile(serialFatTileData);
            }
        }

        public void RemoveFatTile(Vector3Int worldPosition)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.RemoveFatTile(worldPosition);
            }
        }

        public Option<SerialFatTileData> GetFatTileAt(Vector3Int worldPosition)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                return chunk.GetFatTileAt(worldPosition);
            }

            return None<SerialFatTileData>();
        }
        
        /*
         * Returns a tile in a given location
         */
        public Option<SerialTileData> GetTileData(Vector3Int worldPosition)
        {
            var chunkResult = ChunkWithPosition(worldPosition);
            if (chunkResult.IsSome(out var chunk))
            {
                return chunk.GetTileData(worldPosition);
            }
            
            return None<SerialTileData>();
        }
        
        private void ChunkRefresh()
        {
            if (playerObject == null)
                return;

            // TODO: remove this Utils.Utils
            int playerChunk = Utils.Utils.GetChunkXFromPosition(playerObject.transform.position);
            List<int> inRangeOfPlayer = new List<int>();   // chunks that will be loaded based on player pos
            HashSet<Chunk> toBeUnLoaded = new HashSet<Chunk>();     // chunk x coords that will be unloaded based on player pos   

            for (int i = playerChunk - EiramTypes.RENDER_DISTANCE; i <= playerChunk + EiramTypes.RENDER_DISTANCE; i++)
            {
                inRangeOfPlayer.Add(i);
            }

            // if the chunks in the active chunk pool are no longer in range
            // then set them to be inactive
            foreach (var chunk in activeChunks.Values)
            {
                if (!inRangeOfPlayer.Contains(chunk.ChunkX))
                {
                    toBeUnLoaded.Add(chunk);
                }
            }

            // then generate a chunk for each chunkX that is in range
            // of the player but not is not loaded
            foreach (var chunkXInRage in inRangeOfPlayer)
            {
                if (!activeChunks.TryGetValue(chunkXInRage, out Chunk _))
                {
                    var chunk = CreateChunk(chunkXInRage);
                    activeChunks.Add(chunkXInRage, chunk);
                }
            }

            // remove all entries of the chunks that are off-loaded to disk
            foreach (var chunk in toBeUnLoaded)
            {
                activeChunks.Remove(chunk.ChunkX);
                chunk.Die();
            }
        }
        
        private void SaveAllData()
        {
            foreach (var chunk in activeChunks.Values)
            {
                var chunkData = chunk.SerializableData();
                Filesystem.SaveTo(chunkData, $"{chunkData.ChunkX}.chunk", Save.Region);
            }
        }

        private void RandomUpdateChunks()
        {
            foreach(var keyValuePair in activeChunks)
            {
                keyValuePair.Value.RandomUpdate();
            }
        }

        private Chunk CreateChunk(int chunkX)
        {
            if (Save.Region.GetFile($"{chunkX}.chunk").IsSome(out var file))
            {
                var loadResult = Filesystem.LoadFrom<ChunkData>($"{chunkX}.chunk", Save.Region);
                if (loadResult.IsNone)
                {
                    // delete bad data
                    file.Delete();
                }
                else
                {
                    return new Chunk(loadResult.Value);
                }
            }
            
            return new Chunk(chunkX);
        }

        private Option<Chunk> ChunkWithPosition(Vector3Int worldPosition)
        {
            if (worldPosition.y < 0 || worldPosition.y >= EiramTypes.CHUNK_HEIGHT)
                return None<Chunk>();

            // verify chunk is loaded 
            return IsChunkLoaded(Utils.Utils.GetChunkXFromPosition(worldPosition));
        }

        private Option<Chunk> IsChunkLoaded(int chunkX)
        {
            var exists = activeChunks.TryGetValue(chunkX, out Chunk chunk);
            return exists ? new Option<Chunk>(chunk) : None<Chunk>();
        }
        
        private void OnTilePlace(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            Register.GetTileByTileId(serialTileData.TileId).OnPlace(worldPosition, serialTileData);
        }
        
        private void OnTileBreak(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            var tile = Register.GetTileByTileId(serialTileData.TileId);
            var dropsItemIds = tile.GenerateDrops(serialTileData);
            tile.OnBreak(worldPosition, serialTileData);
            
            var spawnOffset = new Vector3(0.5f, 0.5f, 0.0f);

            foreach (var itemId in dropsItemIds)
            {
                var newItemEntity = Instantiate(itemEntityPrefab, worldPosition + spawnOffset, new Quaternion()).GetComponent<ItemEntity>();
                newItemEntity.Init(itemId);
            }
        }
    }
}
        