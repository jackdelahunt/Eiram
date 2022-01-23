using Eiram;
using UnityEngine;

namespace Chunks
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/Lode")]
    public class Lode : ScriptableObject
    {
        public TileId TileId;
        public int MinHeight;
        public int MaxHeight;
        public float Scale;
        public float Threshold;
    }
}