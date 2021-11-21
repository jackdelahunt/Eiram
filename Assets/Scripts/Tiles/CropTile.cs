using System;
using System.Collections.Generic;
using Eiram;
using Events;
using Registers;
using Tags;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Worlds;

namespace Tiles
{
    public abstract class CropTile : DynamicTile
    {
        protected CropTile(ConcreteTileData concreteTileData) : base(concreteTileData)
        {
#if UNITY_EDITOR
            if (concreteTileData is CropTileData cropTileData)
            {
                if(cropTileData.maxAge != cropTileData.tileBases.Count - 1)
                    throw new Exception($"Crop tile max age needs to match (sprite count - 1) => sprites:{cropTileData.tileBases.Count} maxAge:{cropTileData.maxAge}");
            }
            else
            {
                throw new Exception($"Crop tile needs a dynamic tile {concreteTileData.TileName}");
            }
#endif
        }

        public override bool CanPlace(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            if (World.Current.GetTileData(worldPosition.Down()).IsSome(out var serialTileData))
            {
                return serialTileData.TileId == Eiram.TileId.TILLED_SOIL;
            }

            return false;
        }

        public override void OnUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnUpdate(worldPosition, currentTileData);
            if (!CanPlace(worldPosition, currentTileData))
            {
                World.Current.RemoveTileAt(worldPosition);
            }
        }

        public override void OnRandomUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnRandomUpdate(worldPosition, currentTileData);
            OnGrow(worldPosition, currentTileData);
        }

        public virtual void OnGrow(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            int startAge = currentTileData.Tag.GetInt("age");
            int maxAge = CropTileData.maxAge;
            
            if (startAge < maxAge)
                currentTileData.Tag.SetInt("age", startAge + 1);
            
            // if we did not grow this frame
            if (startAge != maxAge)
                RefreshTile(worldPosition, startAge + 1);
        }

        public override List<ItemId> GenerateDrops(SerialTileData currentTileData)
        {
            int age = currentTileData.Tag.GetInt("age");
            int maxAge = CropTileData.maxAge;
 
            // if not full grown drop seed
            if (age != maxAge)
                return new List<ItemId>() {CropTileData.SeedItemId};
            
            // fully grown? drop normal drops
            return base.GenerateDrops(currentTileData);
        }

        public CropTileData CropTileData => this.concreteTileData as CropTileData;
    }
}