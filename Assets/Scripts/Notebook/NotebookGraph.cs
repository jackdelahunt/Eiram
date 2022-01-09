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
        public ItemCountPair[] rewards;
        [Output] public AchievementNode children;
        public static event Action<AchievementStatus> AcheivmentStatusUpdate;

        public List<AchievementNode> ChildAchievements()
        {
            var childs = new List<AchievementNode>();
            foreach (var childPort in GetOutputPort("children").GetConnections())
            {
                childs.Add(childPort.node as AchievementNode);
            }

            return childs;
        }
        
        public void SetStatus(AchievementStatus status)
        {
            this.status = status;
            AcheivmentStatusUpdate?.Invoke(status);
        }
    }

    public enum AchievementStatus
    {
        LOCKED = 0,
        AVAILABLE = 1,
        COMPLETE = 2
    }
}