using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Eiram;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector3Int ChunkPositionToWorldPosition(int x, int y, int chunkX)
        {
            return new Vector3Int(x + (EiramTypes.CHUNK_WIDTH * chunkX), y, 0);
        }
        
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
        
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }
    }
}