namespace Eiram
{
    public static class EiramTypes
    {
        public static readonly int CHUNK_WIDTH = 64;
        public static readonly int CHUNK_HEIGHT = 192;
        public static readonly int RENDER_DISTANCE = 2;
    }
    public enum TileId
    {
        UNKNOWN = -1,
        AIR = 0,
        DIRT = 1,
        GRASS = 2,
        STONE = 3,
        SAND = 4,
        SANDSTONE = 5,
        COBBLESTONE = 6,
        GRAVEL = 7,
        BEDROCK = 8,
        BIRCH_LOG = 9,
        BIRCH_PLANK = 10,
        BIRCH_WOOD = 11,
        BIRCH_LEAVES = 12,
        ROSE = 13,
        DAISY = 14,
        DEADBUSH = 15,
        GRASS_FOLIAGE = 16
    }
}