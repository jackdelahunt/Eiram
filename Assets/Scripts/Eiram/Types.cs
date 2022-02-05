namespace Eiram
{
    public static class EiramTypes
    {
        public static readonly int CHUNK_WIDTH = 64;
        public static readonly int CHUNK_HEIGHT = 192;
        public static readonly int RENDER_DISTANCE = 3;
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
        LOG = 9,
        WOOD = 10,
        PLANK = 11,
        LEAVES = 12,
        ROSE = 13,
        DAISY = 14,
        DEADBUSH = 15,
        GRASS_FOLIAGE = 16,
        TILLED_SOIL = 17,
        THORNS = 18,
        TRELLIS = 19,
        CHEST = 20,
        COPPER = 21,
        SCRAP = 22,
        MINI_TREE = 23,
    }
    
    public enum ItemId
    {
        UNKNOWN = -1,
        DIRT = 0,
        GRASS = 1,
        THORNS = 2,
        TRELLIS = 3,
        CHEST = 4,
        WOOD_SHOVEL = 5,
        WOOD_PICKAXE = 6,
        WOOD_AXE = 7,
        CRANBERRIES = 8,
        STICK = 9,
        WOOD_CLUMP = 10,
        WOOD = 11,
        PLANK = 12,
        ORGANIC_MASS = 13,
        MINI_TREE = 14,
    }
    
    public enum BiomeId
    {
        UNKNOWN = -1,
        GRASS_HILLS = 0,
        STONE_FLATS = 1
    }
    
    public enum ToolType
    {
        NONE = -1,
        PICKAXE = 0,
        AXE = 1,
        SHOVEL = 2
    }
    
    public enum ToolLevel
    {
        NONE = -1,
        WOOD = 0,
        STONE = 1,
    }
}