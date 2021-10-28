using UnityEngine;

namespace Eiram
{
    public static class Extensions
    {
        public static Vector3Int Up(this Vector3Int vector3Int)
        {
            return vector3Int + Vector3Int.up;
        }

        public static Vector3Int Down(this Vector3Int vector3Int)
        {
            return vector3Int + Vector3Int.down;
        }

        public static Vector3Int Left(this Vector3Int vector3Int)
        {
            return vector3Int + Vector3Int.left;
        }

        public static Vector3Int Right(this Vector3Int vector3Int)
        {
            return vector3Int + Vector3Int.right;
        }
    }
}