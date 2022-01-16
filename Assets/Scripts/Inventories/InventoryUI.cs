using System;
using System.Collections.Generic;
using Eiram;
using Events;
using Items;
using Players;
using Registers;
using UnityEngine;
using UnityEngine.UI;
using static Eiram.Handles;
using Object = UnityEngine.Object;

namespace Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] protected GameObject slotPrefab = null;
        [SerializeField] protected GameObject inventoryItemPrefab = null;
        [SerializeField] protected RectTransform contentTransform = null;

        protected Canvas canvas;
        protected List<ItemSlot> itemSlots = new List<ItemSlot>(PlayerInventory.Slots);
        protected Option<Inventory> activeInventory = None<Inventory>();
        protected bool toggled = false;

        protected virtual void Awake()
        {
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            GenerateUI();
        }

        protected virtual void Update()
        {
            if (activeInventory.IsSome(out var inventory) && inventory.IsDirty)
            {
                Refresh();
            }
        }
        
        public void ItemPopped(int slotNumber)
        {
            if (activeInventory.IsSome(out var inventory))
            {
                inventory.ClearSlot(slotNumber);
            }
        }

        public void ItemPlaced(int itemSlot, ItemStack itemStack)
        {
            if (activeInventory.IsSome(out var inventory))
            {
                inventory.ItemStacks[itemSlot] = itemStack;
                inventory.IsDirty = true;
            }
        }

        protected void GenerateUI()
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

        protected void Refresh()
        {
            if (activeInventory.IsSome(out var inventory)) ;
            else return;
            
            for(int i = 0; i < inventory.ItemStacks.Count; i++)
            {
                var itemStack = inventory.ItemStacks[i];
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

        public virtual void OpenInventory()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void CloseInventory()
        {
            gameObject.SetActive(true);
        }
    }
}