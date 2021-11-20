using System;
using System.Collections.Generic;
using Eiram;
using Events;
using Registers;
using Tags;
using Tilemaps;
using UnityEngine;
using Worlds;

namespace Tiles
{
    public abstract class CropTile : AbstractTile
    {
        protected CropTile(ConcreteTileData concreteTileData) : base(concreteTileData)
        {

            defaultTileData = new SerialTileData()
            {
                TileId = concreteTileData.TileId,
                Tag = new Tag(
                        ("age", 0)
                    )
            };
            
#if UNITY_EDITOR
            if (concreteTileData is DynamicTile dynamicTile)
            {
                if(dynamicTile.maxAge != dynamicTile.tileBases.Count)
                    throw new Exception($"Crop tile max age needs to match sprite count -> sprites:{dynamicTile.tileBases.Count} maxAge:{dynamicTile.maxAge}");
            }
            else
            {
                throw new Exception($"Crop tile needs a dynamic tile {concreteTileData.TileName}");
            }
#endif
        }

        public override void OnUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnUpdate(worldPosition, currentTileData);
            if (World.Current.GetTileData(worldPosition.Down()).IsSome(out var serialTileData))
            {
                if (serialTileData.TileId != Eiram.TileId.TILLED_SOIL)
                {
                    World.Current.RemoveTileAt(worldPosition);
                }
            }
        }

        public override void OnRandomUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnRandomUpdate(worldPosition, currentTileData);
            OnGrow(worldPosition, currentTileData);
            var copy = Register.GetTileByTileId(Eiram.TileId.THORNS).DefaultTileData();
            return;
        }

        public virtual void OnGrow(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            int age = currentTileData.Tag.GetInt("age");
            int maxAge = Register.GetTileByTileId(currentTileData.TileId).As<DynamicTile>().Unwrap("This tile is not dynamic").maxAge;
            
            if (age < maxAge)
                currentTileData.Tag.SetInt("age", ++age);
            
            if (age != maxAge)
                RefreshTile(worldPosition, age);
        }

        public virtual void RefreshTile(Vector3Int worldPosition, int age)
        {
            EiramTilemap.Foreground.SetTile(worldPosition, DynamicTile.tileBases[age]);
        }
        
        public DynamicTile DynamicTile => this.concreteTileData as DynamicTile;
    }
}