using System;
using Eiram;
using Items;
using Registers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Eiram.Handles;

namespace Inventories
{
    public class InventoryItem : CountableItem, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public ItemStack ItemStack;
        public Option<ItemSlot> ItemSlot = None<ItemSlot>();
    
        [SerializeField] private float onDragAlpha = 1.0f;
        [SerializeField] private Color MaxDurabilityColour;
        [SerializeField] private Color MinDurabilityColour;
    
        private Option<ItemSlot> lastItemSlot = None<ItemSlot>();
        private RectTransform rectTransform = null;
        private Canvas canvas = null;
        private CanvasGroup canvasGroup = null;

        private bool isItemATool = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            Image = GetComponent<Image>();
        }

        private void Update()
        {
            if (isItemATool)
            {
                SetDurabilityText();
            }
        }

        public void Refresh()
        {
            var item = Register.GetItemByItemId(ItemStack.ItemId); 
            Image.sprite = item.Sprite();
            if (item.IsToolItem(out var _, out var _))
            {
                isItemATool = true;
                Count.text = "";
                SetDurabilityText();
            }
            else
            {
                isItemATool = false;
                Durability.text = "";
                Count.text = ItemStack.Size.ToString();
            }
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

            if (ItemSlot.IsSome(out var slot))
            {
                slot.ItemPopped();
                lastItemSlot = Some(slot);
                ItemSlot = None<ItemSlot>();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (ItemSlot.IsNone)
            {
                ItemSlot = lastItemSlot;
                ItemSlot.Value.ItemPlaced(this);
            }
            else
            {
                lastItemSlot = None<ItemSlot>();
            }
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;
        }

        private void SetDurabilityText()
        {
            Debug.Assert(ItemStack is RichItemStack);
            var richItemStack = ItemStack as RichItemStack;
            isItemATool = true;
            int durability = richItemStack.Tag.GetInt("durability");
            int maxDurability = durability;
            if(Register.GetItemByItemId(ItemStack.ItemId).IsToolItem(out var _, out var toolItemData))
            {
                maxDurability = toolItemData.durability;
            }
            Durability.text = durability.ToString();
            Durability.color = Color.Lerp(MinDurabilityColour, MaxDurabilityColour, (float)durability / (float)maxDurability);
        }
    }
}
