using Eiram;
using Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public abstract class AbstractTile
    {
        private ConcreteTileData concreteTileData;

        public AbstractTile(ConcreteTileData concreteTileData)
        {
            this.concreteTileData = concreteTileData;
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
        public Air(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }

    public class Dirt : AbstractTile, IItemEntityProvider
    {
        public Dirt(ConcreteTileData concreteTileData) : base(concreteTileData){}

        public ItemId ItemId()
        {
            return Eiram.ItemId.DIRT;
        }
    }
    
    public class Grass : AbstractTile
    {
        public Grass(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Stone : AbstractTile
    {
        public Stone(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Bedrock : AbstractTile
    {
        public Bedrock(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
}
