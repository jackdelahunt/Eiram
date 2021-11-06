using System;
using System.Collections;
using System.Collections.Generic;
using Eiram;
using Events;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
    private InventoryItem inventoryItem;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
    }

    public void Refresh()
    {
        if(inventoryItem != null)
            inventoryItem.Refresh();
    }

    public bool IsEmpty()
    {
        return inventoryItem == null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherTransform = eventData.pointerDrag.GetComponent<RectTransform>();
        otherTransform.SetParent(rectTransform);
        //otherTransform.anchoredPosition = rectTransform.anchoredPosition;
        otherTransform.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
        inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
    }

}
