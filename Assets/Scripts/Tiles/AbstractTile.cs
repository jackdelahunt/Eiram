using Eiram;
using Items;
using Players;
using Tags;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Worlds;

namespace Tiles
{
    public abstract class AbstractTile
    {
        private ConcreteTileData concreteTileData;
        protected SerialTileData defaultTileData;

        public AbstractTile(ConcreteTileData concreteTileData)
        {
            this.concreteTileData = concreteTileData;
            defaultTileData = new SerialTileData
            {
                TileId = concreteTileData.TileId,
                Tag = new Tag()
            };
        }

        public virtual void OnUpdate(Vector3Int worldPosition, SerialTileData currentTileData) {}

        public virtual void OnPlace(Vector3Int worldPosition, SerialTileData serialTileData)
        {
            OnUpdate(worldPosition, serialTileData);
            UpdateNeighbours(worldPosition);
        }

        public virtual void OnBreak(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            UpdateNeighbours(worldPosition);
        }

        public virtual void OnUse(Vector3Int worldPosition, SerialTileData currentTileData, Player player) {}

        private void UpdateNeighbours(Vector3Int worldPosition)
        {
            World.Current.UpdateTileAt(worldPosition.Up());
            World.Current.UpdateTileAt(worldPosition.Right());
            World.Current.UpdateTileAt(worldPosition.Down());
            World.Current.UpdateTileAt(worldPosition.Left());
        }

        public SerialTileData DefaultTileData()
        {
            return this.defaultTileData.Clone() as SerialTileData;
        }

        public TileId TileId()
        {
            return concreteTileData.TileId;
        }

        public TileBase TileBase()
        {
            return concreteTileData.TileBase;
        }

        public ItemId ItemId()
        {
            return concreteTileData.ItemId;
        }
    }
    
    public class Air : AbstractTile
    {
        public Air(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }

    public class Dirt : AbstractTile
    {
        public Dirt(ConcreteTileData concreteTileData) : base(concreteTileData){}

        public override void OnUse(Vector3Int worldPosition, SerialTileData currentTileData, Player player)
        {
            base.OnUse(worldPosition, currentTileData, player);
            World.Current.ReplaceTileAt(worldPosition, Eiram.TileId.TILLED_SOIL);
        }
    }
    
    public class Grass : AbstractTile
    {
        public Grass(ConcreteTileData concreteTileData) : base(concreteTileData) {}

        public override void OnUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnUpdate(worldPosition, currentTileData);
            var above = World.Current.GetTileData(new Vector3Int(worldPosition.x, worldPosition.y + 1, 0)).Unwrap();
            if (World.Current.GetTileData(new Vector3Int(worldPosition.x, worldPosition.y + 1, 0)).Unwrap().TileId != Eiram.TileId.AIR)
            {
                World.Current.ReplaceTileAt(worldPosition, Eiram.TileId.DIRT);
            }
        }
    }
    
    public class Stone : AbstractTile
    {
        public Stone(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Bedrock : AbstractTile
    {
        public Bedrock(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class TilledSoil : AbstractTile
    {
        public TilledSoil(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
}
