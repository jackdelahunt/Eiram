using System;
using System.Collections.Generic;
using Eiram;
using Recipes;
using Tiles;
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
        public AchievementStatus status;
        public ItemCountPair[] requirements;
        public ItemCountPair[] rewards;
        [Output] public AchievementNode children;
        public List<AchievementNode> ChildAchievements()
        {
            var childs = new List<AchievementNode>();
            foreach (var childPort in GetOutputPort("children").GetConnections())
            {
                childs.Add(childPort.node as AchievementNode);
            }

            return childs;
        }
    }

    public enum AchievementStatus
    {
        LOCKED = 0,
        AVAILABLE = 1,
        COMPLETE = 2
    }
}