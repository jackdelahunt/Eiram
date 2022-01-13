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
    
        private Option<ItemSlot> lastItemSlot = None<ItemSlot>();
        private RectTransform rectTransform = null;
        private Canvas canvas = null;
        private CanvasGroup canvasGroup = null;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            Image = GetComponent<Image>();
        }

        public void Refresh()
        {
            Count.text = ItemStack.Size.ToString();
            Image.sprite = Register.GetItemByItemId(ItemStack.ItemId).sprite;
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
            Debug.Log("drag end");

        }
    }
}
