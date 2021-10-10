using Eiram;
using Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public abstract class Tile
    {
        private ConcreteTileData concreteTileData;

        public Tile(ConcreteTileData concreteTileData)
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
    
    public class Air : Tile
    {
        public Air(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }

    public class Dirt : Tile, IItemEntityProvider
    {
        public Dirt(ConcreteTileData concreteTileData) : base(concreteTileData){}

        public ItemId ItemId()
        {
            return Eiram.ItemId.DIRT;
        }
    }
    
    public class Grass : Tile
    {
        public Grass(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Stone : Tile
    {
        public Stone(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
    
    public class Bedrock : Tile
    {
        public Bedrock(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
}
