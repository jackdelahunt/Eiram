using System;
using System.Collections;
using System.Collections.Generic;
using Inventories;
using Notebook;
using Players;
using Registers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Notebook
{
    public class AchievementNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Color lockedColour;
        [SerializeField] private Color availableColour;
        [SerializeField] private Color completeColour;
        [SerializeField] private TMP_Text title = null;
        [SerializeField] private Image thumbnail = null;
        [SerializeField] private Image background = null;
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private GameObject hoverCard = null;
        [SerializeField] private ScrollableListUI requirementsList = null;
        [SerializeField] private ScrollableListUI rewardList = null;
        [SerializeField] private GameObject countableItemPrefab = null;
        private Achievement achievement;

        private AchievementStatus lastStatus;

        public void Start()
        {
            hoverCard.SetActive(false);
        }

        public void Update()
        {
            if(lastStatus != achievement.status)
                Refresh();

            lastStatus = achievement.status;
        }

        public void Init(Achievement achievement)
        {
            this.achievement = achievement;
            lastStatus = this.achievement.status;
            title.text = this.achievement.title;
            thumbnail.sprite = this.achievement.thumbnail;
            description.text = this.achievement.description;
            background.color = this.achievement.status switch
            {
                AchievementStatus.LOCKED => lockedColour,
                AchievementStatus.AVAILABLE => availableColour,
                AchievementStatus.COMPLETE => completeColour,
                _ => Color.black
            };

            foreach (var itemCountPair in this.achievement.requirements)
            {
                var icon = requirementsList.Add(countableItemPrefab);
                var countableItem = icon.GetComponent<CountableItem>();
                countableItem.Image.sprite = Register.GetItemByItemId(itemCountPair.ItemId).Sprite();
                countableItem.Count.text = itemCountPair.Amount.ToString();
            }
            
            foreach (var itemCountPair in this.achievement.rewards)
            {
                var icon = rewardList.Add(countableItemPrefab);
                var countableItem = icon.GetComponent<CountableItem>();
                countableItem.Image.sprite = Register.GetItemByItemId(itemCountPair.ItemId).Sprite();
                countableItem.Count.text = itemCountPair.Amount.ToString();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(achievement.status != AchievementStatus.LOCKED)
                hoverCard.SetActive(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCard.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (achievement.status != AchievementStatus.AVAILABLE)
                return;
            
            var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerInventory;
            foreach (var itemCountPair in achievement.requirements)
            {
                if(playerInventory.CountOf(itemCountPair.ItemId) < itemCountPair.Amount)
                    return;
            }

            foreach (var itemCountPair in achievement.rewards)
            {
                playerInventory.TryAddItem(itemCountPair.ItemId, itemCountPair.Amount);
            }

            achievement.status = AchievementStatus.COMPLETE;
            MakeChildrenAvailable();
        }

        private void MakeChildrenAvailable()
        {
            foreach (var child in achievement.children)
            {
                child.status = AchievementStatus.AVAILABLE;
            }
        }

        private void Refresh()
        {
            background.color = achievement.status switch
            {
                AchievementStatus.LOCKED => lockedColour,
                AchievementStatus.AVAILABLE => availableColour,
                AchievementStatus.COMPLETE => completeColour,
                _ => Color.black
            };
        }
    }
}
