using System.Collections.Generic;
using Chunks;
using Eiram;
using UnityEngine;

namespace Worlds
{
    public class World : MonoBehaviour
    {
        public static World Current = null;
        
        private GameObject playerObject;
        private Dictionary<int, Chunk> activeChunks = new Dictionary<int, Chunk>();

        private void Awake()
        {
            Current = this;
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        void Start()
        {
            InvokeRepeating(nameof(ChunkRefresh), 0.0f, 1.0f);
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
        
    }
}
        