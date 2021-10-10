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
        private Item item;
        
        public void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ItemId itemId)
        {
            item = Register.GetItemById(itemId);
            spriteRenderer.sprite = item.sprite;
        }
    }
}