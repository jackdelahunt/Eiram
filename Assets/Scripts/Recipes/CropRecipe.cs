using Eiram;
using UnityEngine;

namespace Recipes
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/CropRecipe")]
    public class CropRecipe : ScriptableObject
    {
        public TileId FinalCrop;
        public TileId LeftCrop;
        public TileId RightCrop;
        public bool Strict; // exact positions for left and right crop
    }
}