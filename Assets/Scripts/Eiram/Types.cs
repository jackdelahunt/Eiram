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
        STONE_PLANT = 24,
        CRANBERRY_BUSH = 25,
        GOLD = 26,
        COPPER_PLANT = 27,
        GOLD_PLANT = 28,
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
        COBBLESTONE = 15,
        STONE = 16,
        STONE_SEED = 17,
        STONE_SHOVEL = 18,
        STONE_PICKAXE = 19,
        STONE_AXE = 20,
        COPPER = 21,
        COPPER_SEED = 22,
        COPPER_SHOVEL = 23,
        COPPER_PICKAXE = 24,
        COPPER_AXE = 25,
        GOLD = 26,
        GOLD_SEED = 27,
        GOLD_SHOVEL = 28,
        GOLD_PICKAXE = 29,
        GOLD_AXE = 30 ,
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
        COPPER = 2,
        GOLD = 3,
    }
}