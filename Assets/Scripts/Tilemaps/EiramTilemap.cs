using System;
using System.Collections.Generic;
using Chunks;
using Eiram;
using Registers;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class EiramTilemap : MonoBehaviour
    {
        public static EiramTilemap Foreground = null;
        public static EiramTilemap Background = null;

        public bool IsForeground;

        private Tilemap tilemap;
        private readonly List<TileBase> tileBaseCache = new List<TileBase>();
        private readonly List<Vector3Int> positionsCache = new List<Vector3Int>();

        public void Awake()
        {
            if(IsForeground)
            {
                Foreground = this;
            }
            else
            {
                Background = this;
            }
            
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
                    if(IsForeground)
                        SetTile(worldPos, chunk.GetTileAt(worldPos));
                    else
                        SetTile(worldPos, chunk.GetBackgroundTileAt(worldPos));
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
                    SetTile(worldPos, Register.GetTileByTileId(TileId.AIR).DefaultTileData());
                }
            }
        }

        public void SetTile(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            positionsCache.Add(worldPosition);
            tileBaseCache.Add(Register.GetTileByTileId(serialTileData.TileId).TileBase(serialTileData));
        }
        
        public void SetTile(Vector3Int worldPosition, TileId tileId)
        {
            positionsCache.Add(worldPosition);
            tileBaseCache.Add(Register.GetTileByTileId(tileId).TileBase(Register.GetTileByTileId(tileId).DefaultTileData()));
        }
        
        public void SetTile(Vector3Int worldPosition, TileBase tileBase)
        {
            positionsCache.Add(worldPosition);
            tileBaseCache.Add(tileBase);
        }
    }
}