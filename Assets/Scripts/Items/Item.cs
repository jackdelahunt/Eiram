using Eiram;
using UnityEngine;

namespace Items
{
    namespace Items
    {
        [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/Item")]
        public class Item : ScriptableObject
        {
            public string itemName;
            public ItemId itemId;
            public Sprite sprite;
        }
    }
}