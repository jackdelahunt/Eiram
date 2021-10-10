using System;
using System.Collections.Generic;
using Events;
using Items;
using Registers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventories
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject playerInventoryUIContainer = null;
        [SerializeField] private List<Image> itemSprites = null;
        [SerializeField] private List<TMP_Text> itemCounts = null;
        [SerializeField] private Sprite emptySlotSprite = null;
        
        private bool toggled = false;

        private void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
        }

        private void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;

        }
        
        private void OnPlayerToggleInventoryEvent(PlayerInventory playerInventory)
        {
            Debug.Assert(playerInventory.Slots == itemSprites.Count && playerInventory.Slots == itemCounts.Count);
            if(toggled) CloseInventory(); else OpenInventory(playerInventory);
            toggled = !toggled;
        }

        private void OpenInventory(PlayerInventory playerInventory)
        {
            LeanTween.moveX(gameObject, transform.position.x + 850.0f, 0.4f);
            
            for (int i = 0; i < playerInventory.ItemStacks.Count; i++)
            {
                var currentItemStack = playerInventory.ItemStacks[i];
                if (currentItemStack != ItemStack.Empty)
                {
                    var item = Register.GetItemById(currentItemStack.ItemId);
                    itemSprites[i].sprite = item.sprite;
                    itemCounts[i].gameObject.SetActive(true);
                    itemCounts[i].text = currentItemStack.Size.ToString();
                }
                else
                {
                    itemSprites[i].sprite = emptySlotSprite;
                    itemCounts[i].text = "0";
                    itemCounts[i].gameObject.SetActive(false);
                }
            }
        }
        
        private void CloseInventory()
        {
            LeanTween.moveX(gameObject, transform.position.x - 850.0f, 0.4f);
        }
    }
}