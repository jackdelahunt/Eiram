using System;
using Eiram;
using Registers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class EiramTilemap : MonoBehaviour
    {
        public static EiramTilemap Instance = null;

        private Tilemap tilemap;

        public void Awake()
        {
            Instance = this;
            tilemap = GetComponent<Tilemap>();
        }

        public void SetTile(Vector3Int worldPosition, TileId tileId)
        {
            tilemap.SetTile(worldPosition, Register.GetTileById(tileId).TileBase());
        }
    }
}