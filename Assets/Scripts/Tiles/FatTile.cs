using Events;
using Inventories;
using Players;
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

        public override bool OnUse(Vector3Int worldPosition, SerialTileData currentTileData, Player player)
        {
            base.OnUse(worldPosition, currentTileData, player);
            if (World.Current.GetFatTileAt(worldPosition).IsSome(out var fatTileData))
            {
                return this.OnUse(worldPosition, currentTileData, fatTileData, player);
            }

            Debug.LogError($"Chunk does not contain fat tile at this world position {worldPosition.x} : {worldPosition.y} ");
            return false;
        }

        public virtual bool OnUse(Vector3Int worldPosition, SerialTileData currentTileData, SerialFatTileData serialFatTileData,
            Player player)
        {
            return false;
        }

        public abstract SerialFatTileData SerialFatTileData(Vector3Int worldPosition);
    }
    
    public class Chest : FatTile
    {
        public Chest(ConcreteTileData concreteTileData) : base(concreteTileData) {}

        public override bool OnUse(Vector3Int worldPosition, SerialTileData currentTileData,
            SerialFatTileData serialFatTileData, Player player)
        {
            base.OnUse(worldPosition, currentTileData, serialFatTileData, player);
            Debug.Assert(serialFatTileData is SerialChestTileData);
            EiramEvents.OnPlayerOpenChestEvent((serialFatTileData as SerialChestTileData).ChestInventory);
            return true;
        }

        public override SerialFatTileData SerialFatTileData(Vector3Int worldPosition)
        {
            return new SerialChestTileData()
            {
                X = worldPosition.x, 
                Y = worldPosition.y,
                ChestInventory = new ChestInventory()
            };
        }
    }
}