using System;
using System.Collections.Generic;
using Eiram;
using Items;
using Items.Items;
using Players;
using Registers;
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

        public virtual bool CanPlace(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            return true;
        }
        
        public virtual bool CanBeBrokenBy(ItemId itemId)
        {
            if (itemId == ItemId.UNKNOWN) return false;
            if (concreteTileData.RequiredToolType == ToolType.NONE) return true;
            var item = Register.GetItemByItemId(itemId);
            if (!item.IsToolItem(out var _, out var toolItem)) return false;
            if (toolItem.toolType != concreteTileData.RequiredToolType) return false;
            return toolItem.toolLevel >= concreteTileData.RequiredToolLevel;

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

        public virtual bool OnUse(Vector3Int worldPosition, SerialTileData currentTileData,
            Player player)
        {
            return false;
        }

        public virtual void OnRandomUpdate(Vector3Int worldPosition, SerialTileData currentTileData) {}

        protected void UpdateNeighbours(Vector3Int worldPosition)
        {
            World.Current.UpdateTileAt(worldPosition.Up());
            World.Current.UpdateTileAt(worldPosition.Right());
            World.Current.UpdateTileAt(worldPosition.Down());
            World.Current.UpdateTileAt(worldPosition.Left());
        }

        public virtual List<ItemId> GenerateDrops(SerialTileData currentTileData)
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
        
        public Option<T> Is<T>() where T: AbstractTile
        {
            if (this is T t) return t;
            return None<T>();
        }

        public Option<T> As<T>() where T: ConcreteTileData
        {
            if (concreteTileData is T t) return t;
            return None<T>();
        }

        public SerialTileData DefaultTileData()
        {
            var clone = this.defaultTileData.Clone() as SerialTileData;
            return clone;
        }

        public string TileName() => concreteTileData.TileName;

        public int Hardness() => concreteTileData.Hardness;

        public ToolType RequiredToolType() => concreteTileData.RequiredToolType;

        public ToolLevel RequiredToolLevel() => concreteTileData.RequiredToolLevel;

        public TileId TileId() => concreteTileData.TileId;

        public AudioClip BreakingSound() => concreteTileData.BreakingSound;
        public AudioClip BreakSound() => concreteTileData.BreakSound;

        public virtual TileBase TileBase(SerialTileData currentTileData)
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
    
    public class Cobblestone : AbstractTile
    {
        public Cobblestone(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Bedrock : AbstractTile
    {
        public Bedrock(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Log : AbstractTile
    {
        public Log(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Wood : AbstractTile
    {
        public Wood(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Plank : AbstractTile
    {
        public Plank(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Leaves : AbstractTile
    {
        public Leaves(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class TilledSoil : AbstractTile
    {
        public TilledSoil(ConcreteTileData concreteTileData) : base(concreteTileData)
        {
            defaultTileData = new SerialTileData
            {
                TileId = concreteTileData.TileId,
                Tag = new Tag(("life", 10))
            };
        }

        public override void OnUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnUpdate(worldPosition, currentTileData);
            if (currentTileData.Tag.GetInt("life") <= 0)
            {
                World.Current.ReplaceTileAt(worldPosition, Eiram.TileId.DIRT);
                if (World.Current.GetTileData(worldPosition.Up()).IsSome(out var aboveData))
                {
                    if (Register.GetTileByTileId(aboveData.TileId).Is<CropTile>().IsSome(out var _))
                    {
                        World.Current.RemoveTileAt(worldPosition.Up());
                    }
                }
            }
        }
    }
        
    public class Thorns : CropTile
    {
        public Thorns(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }
    
    public class Trellis : AbstractTile
    {
        public Trellis(ConcreteTileData concreteTileData) : base(concreteTileData) {}

        public override void OnRandomUpdate(Vector3Int worldPosition, SerialTileData currentTileData)
        {
            base.OnRandomUpdate(worldPosition, currentTileData);
            if(!World.Current.GetTileData(worldPosition.Left()).IsSome(out var leftData)) return;
            
            if(!World.Current.GetTileData(worldPosition.Left()).IsSome(out var rightData)) return;

            if (!(Register.GetTileByTileId(leftData.TileId) is CropTile leftTile)) return;
            if (!(Register.GetTileByTileId(leftData.TileId) is CropTile rightTile)) return;

            if (leftData.Tag.GetInt("age") == leftTile.MaxAge() &&
                rightData.Tag.GetInt("age") == rightTile.MaxAge())
            {
                var r = Register.GetCropRecipe(leftTile.TileId(), rightTile.TileId());
                if (r.IsSome(out var recipe))
                {
                    World.Current.ReplaceTileAt(worldPosition, recipe.FinalCrop);
                }
            }
        }

    }
    
    public class Copper : AbstractTile
    {
        public Copper(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Scrap : AbstractTile
    {
        public Scrap(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class MiniTree : CropTile
    {
        public MiniTree(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }
    
    public class StonePlant : CropTile
    {
        public StonePlant(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }

    public class CopperPlant : CropTile
    {
        public CopperPlant(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }

    public class GoldPlant : CropTile
    {
        public GoldPlant(ConcreteTileData concreteTileData) : base(concreteTileData) {}
    }
    
    public class CranberryBush : AbstractTile
    {
        public CranberryBush(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }

    public class Gold : AbstractTile
    {
        public Gold(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
}
