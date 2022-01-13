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

            foreach (var itemCountPair in node.requirements)
            {
                var icon = requirementsList.Add(countableItemPrefab);
                var countableItem = icon.GetComponent<CountableItem>();
                countableItem.Image.sprite = Register.GetItemByItemId(itemCountPair.ItemId).sprite;
                countableItem.Count.text = itemCountPair.Amount.ToString();
            }
            
            foreach (var itemCountPair in node.rewards)
            {
                var icon = rewardList.Add(countableItemPrefab);
                var countableItem = icon.GetComponent<CountableItem>();
                countableItem.Image.sprite = Register.GetItemByItemId(itemCountPair.ItemId).sprite;
                countableItem.Count.text = itemCountPair.Amount.ToString();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverCard.SetActive(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCard.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (node.status != AchievementStatus.AVAILABLE)
                return;
            
            var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerInventory;
            foreach (var itemCountPair in node.requirements)
            {
                if(playerInventory.CountOf(itemCountPair.ItemId) < itemCountPair.Amount)
                    return;
            }

            foreach (var itemCountPair in node.rewards)
            {
                playerInventory.TryAddItem(itemCountPair.ItemId, itemCountPair.Amount);
            }

            node.status = AchievementStatus.COMPLETE;
            Refresh();
        }

        private void Refresh()
        {
            background.color = node.status switch
            {
                AchievementStatus.LOCKED => lockedColour,
                AchievementStatus.AVAILABLE => availableColour,
                AchievementStatus.COMPLETE => completeColour,
                _ => Color.black
            };
        }
    }
}
