using Eiram;
using UnityEngine;
using UnityEngine.EventSystems;
using static Eiram.Handles;

namespace Inventories
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        public Option<InventoryItem> InventoryItemOption = None<InventoryItem>();
        public int slotNumber = -1;
        public InventoryUI InventoryUI;
    
        private RectTransform rectTransform;
    
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Refresh()
        {
            if (InventoryItemOption.IsSome(out var inventoryItem))
            {
                inventoryItem.Refresh();
            }
        }

        public void ItemPopped()
        {
            InventoryItemOption = None<InventoryItem>();
            InventoryUI.ItemPopped(slotNumber);
        }
    
        public void ItemPlaced(InventoryItem inventoryItem)
        {
            InventoryItemOption = inventoryItem;
            InventoryUI.ItemPlaced(slotNumber, inventoryItem.ItemStack);
            AlignInventoryItem();
        }

        public void Clear()
        {
            if (InventoryItemOption.IsSome(out var inventoryItem))
            {
                Destroy(inventoryItem.gameObject);
                InventoryItemOption = None<InventoryItem>();
            }
        }

        public bool IsEmpty()
        {
            return InventoryItemOption.IsNone;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerDrag == null) return;

            if (!eventData.pointerDrag.CompareTag("InventoryItem")) return;
        
        
            InventoryItemOption = eventData.pointerDrag.GetComponent<InventoryItem>();
            InventoryItemOption.Value.ItemSlot = this;
            InventoryUI.ItemPlaced(slotNumber, InventoryItemOption.Value.ItemStack);
            AlignInventoryItem();
        }

        public void AlignInventoryItem()
        {
            if (InventoryItemOption.IsSome(out var inventoryItem))
            {
                var otherTransform = inventoryItem.GetComponent<RectTransform>();
                otherTransform.SetParent(rectTransform);
                otherTransform.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
}
