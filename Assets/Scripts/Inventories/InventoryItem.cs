using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Items;
using Registers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private TMP_Text count = null;
    [SerializeField] private float onDragAlpha = 1.0f;
    
    private RectTransform rectTransform = null;
    private Canvas canvas = null;
    private CanvasGroup canvasGroup = null;
    private Image image = null;
    private ItemStack itemStack;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        image = GetComponent<Image>();
    }

    public void Init(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        Refresh();
    }

    public void Refresh()
    {
        count.text = itemStack.Size.ToString();
        image.sprite = Register.GetItemById(itemStack.ItemId).sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += (eventData.delta / canvas.scaleFactor);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = onDragAlpha;
        Debug.Log("drag start");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
        Debug.Log("drag end");

    }
}
