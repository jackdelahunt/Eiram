using System;
using System.Collections;
using System.Collections.Generic;
using Notebook;
using UnityEngine;

namespace Notebook
{
    public class NotebookUI : MonoBehaviour
    {
        [SerializeField] private NotebookGraph graph;
        [SerializeField] private GameObject achievementNodeUIPrefab = null;

        private AchievementNode root = null;
        private List<AchievementNodeUI> childAchievementNodes = new List<AchievementNodeUI>();

        private void Awake()
        {
            GetRootNode();
            GetAllAchievementNodeUI();
        }

        void Start()
        {
            PopulateUI();
        }

        private void PopulateUI()
        {
            foreach (var node in graph.nodes)
            {
                var aNode = node as AchievementNode;
                Debug.Assert(aNode);
                foreach (var achievementNodeUI in childAchievementNodes)
                {
                    if (achievementNodeUI.gameObject.name.Equals(aNode.title))
                    {
                        achievementNodeUI.Init(aNode);
                    }
                }
            }
        }

        private void GetRootNode()
        {
            if(graph.nodes.Count == 0) return;
            
            foreach (var node in graph.nodes)
            {
                if (node.GetInputPort("parent").ConnectionCount == 0)
                {
                    root = node as AchievementNode;
                    break;
                }
            }
        }

        private void GetAllAchievementNodeUI(){
            for (int i = 0; i < transform.childCount; i++)
            {
                var childAchievementNode = transform.GetChild(i).GetComponent<AchievementNodeUI>();
                if(childAchievementNode != null)
                    childAchievementNodes.Add(childAchievementNode);
            }
        }
    }
}
