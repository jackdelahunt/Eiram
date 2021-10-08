using Eiram;
using UnityEngine;

namespace Utils
{
    public static class Convert
    {
        public static Vector2Int WorldPositionToChunkPosition(Vector3Int worldPosition)
        {
            return new Vector2Int(worldPosition.x / EiramTypes.CHUNK_WIDTH, worldPosition.y);
        }
    }
}