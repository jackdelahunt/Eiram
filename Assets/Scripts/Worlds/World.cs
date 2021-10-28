using System;
using System.Collections.Generic;
using Chunks;
using Eiram;
using Events;
using Items;
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
        private Dictionary<int, Chunk> activeChunks = new Dictionary<int, Chunk>();

        private void Awake()
        {
            EiramEvents.TileBreakEvent += OnTileBreak;
            Current = this;
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        void Start()
        {
            InvokeRepeating(nameof(ChunkRefresh), 0.0f, 1.0f);
        }

        private void OnDestroy()
        {
            EiramEvents.TileBreakEvent -= OnTileBreak;
        }

        void ChunkRefresh()
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
                    var chunk = new Chunk(chunkXInRage);
                    activeChunks.Add(chunkXInRage, chunk);
                }
            }

            // remove all entries of the chunks that are off-loaded to disk
            foreach (var chunk in toBeUnLoaded)
            {
                chunk.Die();
                activeChunks.Remove(chunk.ChunkX);
            }
        }
        public void PlaceTileAt(Vector3Int worldPosition, TileId tileId)
        {
            var chunkX = Utils.Utils.GetChunkXFromPosition(worldPosition);
            if (activeChunks.TryGetValue(chunkX, out var chunk))
            {
                chunk.PlaceTileAt(worldPosition, tileId);
                Register.GetTileByTileId(tileId).OnPlace(worldPosition, chunk.GetTileData(worldPosition));
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
        
        /*
         * Returns a tile in a given location
         */
        public Some<SerialTileData> GetTileData(Vector3Int worldPosition)
        {
            var chunkResult = ChunkWithPosition(worldPosition);
            if (chunkResult.IsSome(out var chunk))
            {
                return chunk.GetTileData(worldPosition);
            }
            
            return None;
        }
        
        /*
         * returns a chunks that is loaded that contains
         * the position, returns none if it is not loaded
         * or an invalid position
         */
        private Some<Chunk> ChunkWithPosition(Vector3Int worldPosition)
        {
            if (worldPosition.y < 0 || worldPosition.y >= EiramTypes.CHUNK_HEIGHT)
                return None;

            // verify chunk is loaded 
            return IsChunkLoaded(Utils.Utils.GetChunkXFromPosition(worldPosition));
        }

        /*
         * returns a chunk if the chunk 
         * with the given x position is loaded
         */
        private Some<Chunk> IsChunkLoaded(int chunkX)
        {
            var exists = activeChunks.TryGetValue(chunkX, out Chunk chunk);
            return exists ? new Some<Chunk>(chunk) : None;
        }

        private void OnTileBreak(Vector3Int worldPosition, TileId tileId)
        {
            ItemId itemId = Register.GetTileByTileId(tileId).ItemId();
            if (itemId != ItemId.UNKNOWN)
            {
                var spawnOffset = new Vector3(0.5f, 0.5f, 0.0f);
                var newItemEntity = Instantiate(itemEntityPrefab, worldPosition + spawnOffset, new Quaternion()).GetComponent<ItemEntity>();
                newItemEntity.Init(itemId);
            }
        }
    }
}
        