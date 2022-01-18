using Eiram;
using Items.Items;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Eiram", menuName = "Eiram/ToolItem")]
    public class ToolItemData : ConcreteItemData
    {
        public ToolType toolType;
        public ToolLevel toolLevel;
    }
}