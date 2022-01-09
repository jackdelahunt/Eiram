using Eiram;
using Recipes;
using UnityEngine;
using XNode;

namespace Notebook
{
    [CreateAssetMenu]
    public class NotebookGraph: NodeGraph {}
    
    public class AchievementNode : Node
    {
        [Input] public AchievementNode parent;
        public string title;
        public string description;
        public ItemCountPair[] rewards;
        [Output] public AchievementNode children;
    }
}