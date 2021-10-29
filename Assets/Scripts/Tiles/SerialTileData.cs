using System;
using Eiram;
using Tags;

namespace Tiles
{
    [Serializable]
    public class SerialTileData : ICloneable
    {
        public SerialTileData()
        {
            TileId = TileId.UNKNOWN;
            Tag = new Tag();
        }
        
        public object Clone()
        {
            return new SerialTileData
            {
                TileId = this.TileId,
                Tag = Tag
            };
        }
        
        public TileId TileId;
        public Tag Tag;
    }
}