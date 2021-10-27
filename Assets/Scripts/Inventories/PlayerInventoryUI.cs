using System;
using System.Collections.Generic;
using Eiram;
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
        [SerializeField] private Vector3 pointerOffset = new Vector3();
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private RectTransform contentTransform;
        [SerializeField] private Sprite emptySlotSprite = null;
        [SerializeField] private GameObject slotPointer = null;
        
        private List<Image> itemSprites = new List<Image>(PlayerInventory.Slots);
        private List<TMP_Text> itemCounts = new List<TMP_Text>(PlayerInventory.Slots);
        
        private bool toggled = false;

        private void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
            EiramEvents.PlayerInventoryIsDirtyEvent += OnPlayerInventoryIsDirty;
            EiramEvents.SelectedSlotChangedEvent += OnSelectedSlotChanged;
            GenerateUI();
        }

        private void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
            EiramEvents.PlayerInventoryIsDirtyEvent -= OnPlayerInventoryIsDirty;
            EiramEvents.SelectedSlotChangedEvent -= OnSelectedSlotChanged;
        }

        private void GenerateUI()
        {
            for (int i = 0; i < PlayerInventory.Slots; i++)
            {
                var go = Instantiate(slotPrefab, contentTransform);
                itemSprites.Add(go.GetComponentInChildren<Image>());
                itemCounts.Add(go.GetComponentInChildren<TMP_Text>());
            }
            
        }

        private void MovePointer(int slotIndex)
        {
            var pos = itemSprites[slotIndex].gameObject.transform.position;
            Debug.Log(pos);
            slotPointer.transform.position = pos + pointerOffset;
        }

        private void OnPlayerToggleInventoryEvent(PlayerInventory playerInventory)
        {
            Debug.Assert(PlayerInventory.Slots == itemSprites.Count && PlayerInventory.Slots == itemCounts.Count);
            if(toggled) CloseInventory(); else OpenInventory(playerInventory);
            toggled = !toggled;
        }

        private void OnPlayerInventoryIsDirty(PlayerInventory playerInventory)
        {
            Refresh(playerInventory);
        }

        private void OnSelectedSlotChanged(PlayerInventory playerInventory, int slotIndex)
        {
            MovePointer(slotIndex);
        }

        private void Refresh(PlayerInventory playerInventory)
        {
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

        private void OpenInventory(PlayerInventory playerInventory)
        {
            LeanTween.moveY(gameObject, transform.position.y - 460.0f, 0.4f);
            Refresh(playerInventory);
        }
        
        private void CloseInventory()
        {
            LeanTween.moveY(gameObject, transform.position.y + 460.0f, 0.4f);
        }
    }
}