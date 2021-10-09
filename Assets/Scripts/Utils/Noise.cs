using UnityEngine;

namespace Utils
{
    public class Noise
    {
        public static float TerrainNoise(int x, int y, int seed, float scale = 0.05f)
        {
            System.Random prng = new System.Random(seed);
            float seedOffsetX = prng.Next(-100000, 100000);
            float seedOffsetY = prng.Next(-100000, 100000);

            float sampleX = (x * scale) + 0.01f;
            float sampleY = (y * scale) + 0.01f;

            float value = Mathf.PerlinNoise(sampleX + seedOffsetX, sampleY + seedOffsetY);
            return value;
        }

        // returns true or false if the noise form the inputs is above the threshold
        public static bool CaveNoise(int x, int y, float threshold, int seed)
        {
            return TerrainNoise(x, y, seed, 0.2f) > threshold;
        }
    }
}