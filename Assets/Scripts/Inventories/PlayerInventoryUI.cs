using System;
using System.Collections.Generic;
using Events;
using Items;
using Players;
using UnityEngine;
using UnityEngine.UI;
using static Eiram.Handles;

namespace Inventories
{
    public class PlayerInventoryUI : InventoryUI
    {
        [SerializeField] private Vector3 pointerOffset = new Vector3();
        [SerializeField] private GameObject slotPointer = null;
        
        [SerializeField] private RectTransform openPosition;
        [SerializeField] private RectTransform closePosition;

        private PlayerInventory playerInventory => activeInventory.Unwrap() as PlayerInventory;

        protected override void Awake()
        {
            EiramEvents.PlayerTogglePlayerInventoryEvent += OnPlayerTogglePlayerInventory;
            EiramEvents.SelectedSlotChangedEvent += OnSelectedSlotChanged;
            
            AwakeInventoryUI();
            gameObject.SetActive(true);
        }

        private void Start()
        {
            transform.position = closePosition.position;
            Invoke(nameof(LateStart), 0.001f); // TODO: fix this crap
        }

        protected override void Update()
        {
            if (activeInventory.IsSome(out var _) && playerInventory.IsDirty)
            {
                playerInventory.IsDirty = false;
                Refresh();
            }
        }

        private new void OnDestroy()
        {
            EiramEvents.PlayerTogglePlayerInventoryEvent -= OnPlayerTogglePlayerInventory;
            EiramEvents.SelectedSlotChangedEvent -= OnSelectedSlotChanged;
        }
        
        public override void OpenInventory()
        {
            LeanTween.moveY(gameObject, openPosition.position.y, 0.4f);
            toggled = true;
        }
        
        public override void CloseInventory()
        {
            LeanTween.moveY(gameObject, closePosition.position.y, 0.4f);
            toggled = false;
        }
        
        private void LateStart()
        {
            activeInventory = Some(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()
                .playerInventory as Inventory);
            MovePointer(playerInventory.SelectedSlot);
        }

        private void MovePointer(int slotIndex)
        {
            var pos = itemSlots[slotIndex].gameObject.transform.position;
            slotPointer.transform.position = pos + pointerOffset;
        }

        private void OnSelectedSlotChanged(int slotIndex)
        {
            if (!toggled)
            {
                MovePointer(slotIndex);
                if (itemSlots[slotIndex].InventoryItemOption.IsSome(out var item))
                {
                    EiramEvents.OnItemInfoRequestEvent(item.ItemStack.ItemId);
                }
            }
        }

        private void OnPlayerTogglePlayerInventory(PlayerInventory _)
        {
            if (!toggled)
                OpenInventory();   
            else
                CloseInventory();
        }
    }
}