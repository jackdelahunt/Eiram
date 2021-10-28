using Eiram;
using Tags;

namespace Tiles
{
    public class SerialTileData
    {
        public SerialTileData()
        {
            TileId = TileId.UNKNOWN;
            Tag = new Tag();
        }
        
        public TileId TileId;
        public Tag Tag;
    }
}