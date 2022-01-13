using System;
using System.Collections.Generic;
using Events;
using Items;
using Players;
using Registers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories
{
    public class PlayerInventoryUI : MonoBehaviour, IInventoryUI
    {
        [SerializeField] private Vector3 pointerOffset = new Vector3();
        [SerializeField] private GameObject slotPrefab = null;
        [SerializeField] private GameObject inventoryItemPrefab = null;
        [SerializeField] private RectTransform contentTransform = null;
        [SerializeField] private GameObject slotPointer = null;

        private Canvas canvas;
        private List<ItemSlot> itemSlots = new List<ItemSlot>(PlayerInventory.Slots);
        private PlayerInventory playerInventory;
        private bool toggled = false;

        private void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
            EiramEvents.SelectedSlotChangedEvent += OnSelectedSlotChanged;

            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            
            GenerateUI();
        }

        private void Start()
        {
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerInventory;
            Invoke(nameof(LateStart), 0.05f); // TODO: fix this crap
        }

        private void LateStart()
        {
            MovePointer(playerInventory.SelectedSlot);
        }

        private void Update()
        {
            if (playerInventory.IsDirty)
            {
                playerInventory.IsDirty = false;
                Refresh();
            }
        }

        private void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
            EiramEvents.SelectedSlotChangedEvent -= OnSelectedSlotChanged;
        }

        public void ItemPopped(int slotNumber)
        {
            playerInventory.ClearSlot(slotNumber);
        }

        public void ItemPlaced(int itemSlot, ItemStack itemStack)
        {
            playerInventory.ItemStacks[itemSlot] = itemStack;
            playerInventory.IsDirty = true;
        }

        private void GenerateUI()
        {
            for (int i = 0; i < PlayerInventory.Slots; i++)
            {
                var go = Instantiate(slotPrefab, contentTransform);
                var itemSlot = go.GetComponent<ItemSlot>();
                itemSlot.slotNumber = i;
                itemSlot.InventoryUI = this;
                itemSlots.Add(itemSlot);
            }
        }

        private void MovePointer(int slotIndex)
        {
            var pos = itemSlots[slotIndex].gameObject.transform.position;
            slotPointer.transform.position = pos + pointerOffset;
        }

        private void OnPlayerToggleInventoryEvent(PlayerInventory playerInventory)
        {
            Debug.Assert(PlayerInventory.Slots == itemSlots.Count);
            if(toggled) CloseInventory(); else OpenInventory();
            toggled = !toggled;
        }

        private void OnSelectedSlotChanged(int slotIndex)
        {
            MovePointer(slotIndex);
        }

        private void Refresh()
        {
            for(int i = 0; i < playerInventory.ItemStacks.Count; i++)
            {
                var itemStack = playerInventory.ItemStacks[i];
                var itemSlot = itemSlots[i];
                itemSlot.Clear();
                if (!itemStack.IsEmpty())
                {
                    var inventoryItemGo = Instantiate(inventoryItemPrefab, itemSlots[i].transform);
                    var inventoryItem = inventoryItemGo.GetComponent<InventoryItem>();
                    
                    inventoryItem.ItemStack = itemStack;
                    inventoryItem.ItemSlot = itemSlot;
                    
                    itemSlot.InventoryItemOption = inventoryItem;
                }
                
                itemSlot.Refresh();
            }
        }

        private void OpenInventory()
        {
            LeanTween.moveY(gameObject, transform.position.y - 460.0f, 0.4f);
            playerInventory.Sort();
        }
        
        private void CloseInventory()
        {
            LeanTween.moveY(gameObject, transform.position.y + 460.0f, 0.4f);
        }
    }
}