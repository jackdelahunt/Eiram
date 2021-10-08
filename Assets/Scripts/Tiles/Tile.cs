using Eiram;
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

    public class Dirt : Tile
    {
        public Dirt(ConcreteTileData concreteTileData) : base(concreteTileData){}
    }
}
