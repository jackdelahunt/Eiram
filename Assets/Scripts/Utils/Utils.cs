using Eiram;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static int GetChunkXFromPosition(Vector3Int pos)
        {
            int remain = (pos.x % EiramTypes.CHUNK_WIDTH + EiramTypes.CHUNK_WIDTH) % EiramTypes.CHUNK_WIDTH;
            return (pos.x - remain) / EiramTypes.CHUNK_WIDTH;
        }
        
        public static int GetChunkXFromPosition(Vector3 pos)
        {
            int remain = ((int)pos.x % EiramTypes.CHUNK_WIDTH + EiramTypes.CHUNK_WIDTH) % EiramTypes.CHUNK_WIDTH;
            return ((int)pos.x - remain) / EiramTypes.CHUNK_WIDTH;
        }
    }
}