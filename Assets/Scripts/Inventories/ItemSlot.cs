using System;
using System.Collections;
using System.Collections.Generic;
using Eiram;
using Events;
using Inventories;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public InventoryItem inventoryItem;
    public int slotNumber = -1;
    public InventoryUI InventoryUI;
    
    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Refresh()
    {
        if(inventoryItem != null)
            inventoryItem.Refresh();
    }

    public void ItemPopped()
    {
        inventoryItem = null;
        InventoryUI.ItemPopped(slotNumber);
    }
    
    public void ItemPlaced(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        InventoryUI.ItemPlaced(slotNumber, inventoryItem.ItemStack);
        AlignInventoryItem();
    }

    public void Clear()
    {
        if (inventoryItem != null)
        {
            Destroy(inventoryItem.gameObject);
            inventoryItem = null;
        }
    }

    public bool IsEmpty()
    {
        return inventoryItem == null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null) return;

        if (!eventData.pointerDrag.CompareTag("InventoryItem")) return;
        
        
        inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        inventoryItem.ItemSlot = this;
        InventoryUI.ItemPlaced(slotNumber, inventoryItem.ItemStack);
        AlignInventoryItem();
    }

    public void AlignInventoryItem()
    {
        var otherTransform = inventoryItem.GetComponent<RectTransform>();
        otherTransform.SetParent(rectTransform);
        otherTransform.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

}
