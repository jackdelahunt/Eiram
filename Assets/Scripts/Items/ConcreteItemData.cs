using Eiram;
using UnityEngine;

namespace Items
{
    namespace Items
    {
        [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/ConcreteItemData")]
        public class ConcreteItemData : ScriptableObject
        {
            public string itemName;
            public ItemId itemId;
            public TileId tileId;
            public int maxStack = 64;
            public Sprite sprite;
        }
    }
}