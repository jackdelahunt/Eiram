using System;
using Eiram;
using Items.Items;
using Registers;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemEntity : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer = null;
        public ItemId ItemId = ItemId.UNKNOWN;
        public int Size = 0;
        
        public void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ItemId itemId, int size = 1)
        {
            this.ItemId = itemId;
            this.Size = size;
            spriteRenderer.sprite = Register.GetItemByItemId(itemId).sprite;
        }
    }
}