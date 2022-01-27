using Eiram;
using UnityEngine;

namespace Chunks
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/Foliage")]
    public class Foliage : ScriptableObject
    {
        public TileId tileID;
        public float scale;
        public float threshold;
    }
}