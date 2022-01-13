using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Graphics;
using Notebook;
using UnityEngine;

namespace Notebook
{
    public class NotebookUI : MonoBehaviour
    {
        [SerializeField] private GameObject achievementNodeUIPrefab = null;
        [SerializeField] private float achievementFocalLength;
        [SerializeField] private float startYPosition;
        [SerializeField] private float scrollSensitivity;
        [SerializeField] private RectTransform start;
        [SerializeField] private RectTransform end;


        private Achievement root = null;
        private List<AchievementNodeUI> childAchievementNodes = new List<AchievementNodeUI>();
        private bool toggled = false;

        private void Awake()
        {
            EiramEvents.PlayerToggleNotebookEvent += OnNotebookToggleEvent;
            GetAllAchievementNodeUI();
            root = Achievement.NewTree();
        }

        private void Start()
        {
            PopulateUI();
        }
        
        public void Update()
        {
            if(!toggled) return;
            
            float scrollAmount = Input.GetAxisRaw("Scroll");
            if(scrollAmount < -0.001f || scrollAmount > 0.001f)
                ScrollGraph(scrollAmount * -scrollSensitivity);
        }

        private void OnDestroy()
        {
            EiramEvents.PlayerToggleNotebookEvent -= OnNotebookToggleEvent;
        }

        private void ScrollGraph(float amount)
        {
            transform.position += new Vector3(0.0f,  amount * -scrollSensitivity, 0.0f);
        }

        private void PopulateUI()
        {
            foreach (var node in root.AllAchievements())
            {
                foreach (var achievementNodeUI in childAchievementNodes)
                {
                    if (achievementNodeUI.gameObject.name.Equals(node.title))
                    {
                        achievementNodeUI.Init(node);
                    }
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

        private void OnNotebookToggleEvent()
        {
            if (toggled)
            {
                CloseNotebook();
                PostProcessing.instance.DefaultFocalLength();
            }
            else
            {
                OpenNotebook();
                PostProcessing.instance.AchievementFocalLength(achievementFocalLength);
            }

            toggled = !toggled;
        }
        
        private void OpenNotebook()
        {
            LeanTween.moveY(gameObject, start.position.y, 0.4f);
        }
        
        private void CloseNotebook()
        {
            LeanTween.moveY(gameObject, end.position.y, 0.4f);
        }
    }
}
