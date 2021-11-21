using System;
using Eiram;
using Registers;
using Tags;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Worlds;

namespace Tiles
{
    public class DynamicTile : AbstractTile
    {
        protected DynamicTile(ConcreteTileData concreteTileData) : base(concreteTileData)
        {

            defaultTileData = new SerialTileData()
            {
                TileId = concreteTileData.TileId,
                Tag = new Tag(
                        ("age", 0)
                    )
            };
            
#if UNITY_EDITOR
            if (concreteTileData is DynamicTileData dynamicTile)
            {
                if(dynamicTile.maxAge != dynamicTile.tileBases.Count - 1)
                    throw new Exception($"Dynamic tile max age needs to match (sprite count - 1) => sprites:{dynamicTile.tileBases.Count} maxAge:{dynamicTile.maxAge}");
            }
            else
            {
                throw new Exception($"Dynamic tile needs a dynamic tile {concreteTileData.TileName}");
            }
#endif
        }

        public virtual void RefreshTile(Vector3Int worldPosition, int age)
        {
            EiramTilemap.Foreground.SetTile(worldPosition, DynamicTileData.tileBases[age]);
        }

        public override TileBase TileBase(SerialTileData currentTileData)
        {
            int age = currentTileData.Tag.GetInt("age");
            return DynamicTileData.tileBases[age];
        }

        public DynamicTileData DynamicTileData => this.concreteTileData as DynamicTileData;
    }
}