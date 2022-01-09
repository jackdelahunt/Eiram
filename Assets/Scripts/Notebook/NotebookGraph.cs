using System.Collections.Generic;
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
        public Sprite thumbnail;
        public string title;
        public string description;
        public ItemCountPair[] rewards;
        [Output] public AchievementNode children;

        public int depth { get; private set; }

        public List<AchievementNode> ChildAchievements()
        {
            var childs = new List<AchievementNode>();
            foreach (var childPort in GetOutputPort("children").GetConnections())
            {
                childs.Add(childPort.node as AchievementNode);
            }

            return childs;
        }
        
        public void SetDepth(int depth)
        {
            this.depth = depth;
        }
    }
}