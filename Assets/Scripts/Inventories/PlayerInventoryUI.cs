using System;
using System.Collections.Generic;
using Events;
using Items;
using Players;
using Registers;
using UnityEngine;
using UnityEngine.UI;
using static Eiram.Handles;

namespace Inventories
{
    public class PlayerInventoryUI : InventoryUI
    {
        [SerializeField] private Vector3 pointerOffset = new Vector3();
        [SerializeField] private GameObject slotPointer = null;

        private PlayerInventory playerInventory => activeInventory.Unwrap() as PlayerInventory;

        protected override void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
            EiramEvents.SelectedSlotChangedEvent += OnSelectedSlotChanged;
            
            AwakeInventoryUI();
            gameObject.SetActive(true);
        }

        private void Start()
        {
            activeInventory = Some(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()
                .playerInventory as Inventory);
            Invoke(nameof(LateStart), 0.05f); // TODO: fix this crap
        }

        protected override void Update()
        {
            if (playerInventory.IsDirty)
            {
                playerInventory.IsDirty = false;
                Refresh();
            }
        }

        private new void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
            EiramEvents.SelectedSlotChangedEvent -= OnSelectedSlotChanged;
        }
        
        public override void OpenInventory()
        {
            LeanTween.moveY(gameObject, transform.position.y - 460.0f, 0.4f);
            toggled = true;
        }
        
        public override void CloseInventory()
        {
            LeanTween.moveY(gameObject, transform.position.y + 460.0f, 0.4f);
            toggled = false;
        }
        
        private void LateStart()
        {
            MovePointer(playerInventory.SelectedSlot);
        }

        private void MovePointer(int slotIndex)
        {
            var pos = itemSlots[slotIndex].gameObject.transform.position;
            slotPointer.transform.position = pos + pointerOffset;
        }

        private void OnSelectedSlotChanged(int slotIndex)
        {
            if(!toggled)
                MovePointer(slotIndex);
        }

        private void OnPlayerToggleInventoryEvent()
        {
            if (!toggled)
            {
                if(Player.InInventory)
                {
                    toggled = false;
                    return;
                }
                OpenInventory();   
            }
            else
            {
                CloseInventory();
            }
        }
    }
}