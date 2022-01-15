using UnityEngine;
using Worlds;

namespace Tiles
{
    public abstract class FatTile : AbstractTile
    {
        public FatTile(ConcreteTileData concreteTileData) : base(concreteTileData) {}

        public override void OnPlace(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            base.OnPlace(worldPosition, serialTileData);
            World.Current.AddFatTile(worldPosition, this.SerialFatTileData(worldPosition));
        }

        public override void OnBreak(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnBreak(worldPosition, currentTileData);
            World.Current.RemoveFatTile(worldPosition);
        }

        public abstract SerialFatTileData SerialFatTileData(Vector3Int worldPosition);
    }
    
    public class Chest : FatTile
    {
        public Chest(ConcreteTileData concreteTileData) : base(concreteTileData) {}

        public override SerialFatTileData SerialFatTileData(Vector3Int worldPosition)
        {
            return new SerialFatTileData() {X = worldPosition.x, Y = worldPosition.y};
        }
    }
}