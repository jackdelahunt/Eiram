using System;
using System.Collections;
using System.Collections.Generic;
using Notebook;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Notebook
{
    public class AchievementNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color lockedColour;
        [SerializeField] private Color availableColour;
        [SerializeField] private Color completeColour;
        [SerializeField] private TMP_Text title = null;
        [SerializeField] private Image thumbnail = null;
        [SerializeField] private Image background = null;
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private GameObject hoverCard = null;
        private AchievementNode node;

        public void Start()
        {
            hoverCard.SetActive(false);
        }

        public void Init(AchievementNode node)
        {
            this.node = node;
            title.text = this.node.title;
            thumbnail.sprite = this.node.thumbnail;
            description.text = this.node.description;
            background.color = node.status switch
            {
                AchievementStatus.LOCKED => lockedColour,
                AchievementStatus.AVAILABLE => availableColour,
                AchievementStatus.COMPLETE => completeColour,
                _ => Color.black
            };
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverCard.SetActive(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCard.SetActive(false);
        }
    }
}
