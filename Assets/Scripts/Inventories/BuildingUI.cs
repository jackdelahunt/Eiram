using System;
using Eiram;
using Events;
using Recipes;
using Registers;
using UnityEngine;

namespace Inventories
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] private GameObject buildingItemPrefab = null;
        [SerializeField] private RectTransform contentTransform = null;

        private bool toggled = false;
        
        public void Awake()
        {
            EiramEvents.PlayerToggleInventoryEvent += OnPlayerToggleInventoryEvent;
        }

        public void OnBuildingItemClicked(BuildingRecipe recipe)
        {
            Debug.Log(recipe.FinalItem);
        }
        
        private void OnDestroy()
        {
            EiramEvents.PlayerToggleInventoryEvent -= OnPlayerToggleInventoryEvent;
        }

        private void OnPlayerToggleInventoryEvent(PlayerInventory playerInventory)
        {
            if(toggled)
            {
                DestroyRecipeIcons();
                CloseInventory();
            } else
            {
                CreateRecipeIcons(playerInventory);
                OpenInventory();
            }
            
            toggled = !toggled;
        }

        private void CreateRecipeIcons(PlayerInventory inventory)
        {
            foreach (var recipe in Register.GetAllBuildingRecipes())
            {
                if (inventory.CanBuildRecipe(recipe))
                {
                    var icon = Instantiate(buildingItemPrefab, contentTransform).GetComponent<BuildingItemUI>();
                    icon.Init(OnBuildingItemClicked, recipe);
                }
            }
        }
        
        private void DestroyRecipeIcons()
        {
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                Destroy(contentTransform.GetChild(i).gameObject);
            }
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