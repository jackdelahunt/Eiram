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
        private PlayerInventory playerInventory = null;
        
        public void Awake()
        {
            EiramEvents.PlayerTogglePlayerInventoryEvent += OnPlayerTogglePlayerInventoryEvent;
        }

        public void OnBuildingItemClicked(BuildingRecipe recipe)
        {
            if(!playerInventory.CanBuildRecipe(recipe)) return;

            foreach (var ingredient in recipe.Ingredients)
            {
                int remaining = ingredient.Amount;
                var slots = playerInventory.SlotsOfItem(ingredient.ItemId);
                foreach (var slot in slots)
                {
                    int toRemove = remaining > playerInventory.ItemStackAt(slot).Size
                        ? playerInventory.ItemStackAt(slot).Size
                        : remaining; 
                    playerInventory.RemoveFromItemStack(slot, toRemove);
                    remaining -= toRemove;
                    
                    if(remaining <= 0) break;
                }
            }

            playerInventory.TryAddItem(recipe.FinalItem.ItemId, recipe.FinalItem.Amount);
            
            DestroyRecipeIcons();
            CreateRecipeIcons();
        }
        
        private void OnDestroy()
        {
            EiramEvents.PlayerTogglePlayerInventoryEvent -= OnPlayerTogglePlayerInventoryEvent;
        }

        private void OnPlayerTogglePlayerInventoryEvent(PlayerInventory playerInventory)
        {
            if(toggled)
            {
                this.playerInventory = null;
                DestroyRecipeIcons();
                CloseInventory();
            } else
            {
                this.playerInventory = playerInventory;
                CreateRecipeIcons();
                OpenInventory();
            }
            
            toggled = !toggled;
        }

        private void CreateRecipeIcons()
        {
            foreach (var recipe in Register.buildingRecipes)
            {
                if (playerInventory.CanBuildRecipe(recipe))
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