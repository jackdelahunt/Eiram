using System;
using System.Collections.Generic;
using Eiram;
using Items;
using Players;
using Tags;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Worlds;
using Random = System.Random;
using static Eiram.Handles;

namespace Tiles
{
    public abstract class AbstractTile
    {
        protected ConcreteTileData concreteTileData;
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

        public virtual void OnRandomUpdate(Vector3Int worldPosition, SerialTileData currentTileData) {}

        private void UpdateNeighbours(Vector3Int worldPosition)
        {
            World.Current.UpdateTileAt(worldPosition.Up());
            World.Current.UpdateTileAt(worldPosition.Right());
            World.Current.UpdateTileAt(worldPosition.Down());
            World.Current.UpdateTileAt(worldPosition.Left());
        }

        public virtual List<ItemId> GenerateDrops()
        {
            var drops = new List<ItemId>();
            var r = new Random();
            foreach (var drop in concreteTileData.Drops)
            {
                if (r.NextDouble() <= drop.Chance)
                {
                    for(int i = 0; i < drop.Quantity; i++) drops.Add(drop.ItemId);
                }
            }

            return drops;
        }

        public Option<T> As<T>()
        {
            if (concreteTileData is T t) return t;
            return None<T>();
        }

        public SerialTileData DefaultTileData()
        {
            var clone = this.defaultTileData.Clone() as SerialTileData;
            return clone;
        }

        public TileId TileId()
        {
            return concreteTileData.TileId;
        }

        public TileBase TileBase()
        {
            return concreteTileData.TileBase;
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
    
    public class Thorns : CropTile
    {
        public Thorns(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }
}
