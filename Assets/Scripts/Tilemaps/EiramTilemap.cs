using System;
using System.Collections.Generic;
using Chunks;
using Eiram;
using Registers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class EiramTilemap : MonoBehaviour
    {
        public static EiramTilemap Foreground = null;

        private Tilemap tilemap;
        private readonly List<TileBase> tileBaseCache = new List<TileBase>();
        private readonly List<Vector3Int> positionsCache = new List<Vector3Int>();

        public void Awake()
        {
            Foreground = this;
            tilemap = GetComponent<Tilemap>();
        }

        public void Update()
        {
            if (tileBaseCache.Count > 0)
            {
                tilemap.SetTiles(positionsCache.ToArray(), tileBaseCache.ToArray());
                positionsCache.Clear();
                tileBaseCache.Clear();
            }
        }

        public void DrawChunk(Chunk chunk)
        {
            for (int i = 0; i < EiramTypes.CHUNK_WIDTH; i++)
            {
                for (int j = 0; j < EiramTypes.CHUNK_HEIGHT; j++)
                {
                    var worldPos = new Vector3Int((chunk.ChunkX * EiramTypes.CHUNK_WIDTH) + i, j, 0);
                    SetTile(worldPos, chunk.GetTileAt(worldPos));
                }
            }
        }
        
        public void RemoveChunk(Chunk chunk)
        {
            for (int i = 0; i < EiramTypes.CHUNK_WIDTH; i++)
            {
                for (int j = 0; j < EiramTypes.CHUNK_HEIGHT; j++)
                {
                    var worldPos = new Vector3Int((chunk.ChunkX * EiramTypes.CHUNK_WIDTH) + i, j, 0);
                    SetTile(worldPos, TileId.AIR);
                }
            }
        }

        public void SetTile(Vector3Int worldPosition, TileId tileId)
        {
            positionsCache.Add(worldPosition);
            tileBaseCache.Add(Register.GetTileByTileId(tileId).TileBase());
        }
    }
}