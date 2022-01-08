using System;
using Eiram;
using Events;
using UnityEngine;

namespace Inventories
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] private GameObject staticImagePrefab = null;
        [SerializeField] private RectTransform contentTransform = null;

        private bool toggled = false;
        
        public void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
        }

        public void Start()
        {
            var icon = Instantiate(staticImagePrefab, contentTransform).GetComponent<BuildingItemUI>();
            icon.Init(OnBuildingItemClicked, ItemId.GRASS);
        }

        public void OnBuildingItemClicked(ItemId id)
        {
            Debug.Log(id);
        }
        
        private void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
        }

        private void OnPlayerToggleInventoryEvent(PlayerInventory playerInventory)
        {
            if(toggled) CloseInventory(); else OpenInventory();
            toggled = !toggled;
        }
        
        private void OpenInventory()
        {
            LeanTween.moveX(gameObject, gameObject.transform.position.x - 50.0f, 0.4f);
        }
        
        private void CloseInventory()
        {
            LeanTween.moveX(gameObject, gameObject.transform.position.x + 50.0f, 0.4f);
        }
    }
}