using System;
using Eiram;
using Recipes;
using Registers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories
{
    public class BuildingItemUI : MonoBehaviour
    {
        private Image iconImage;
        private Action<BuildingRecipe> callback;
        private BuildingRecipe recipe;

        public void Awake()
        {
            iconImage = GetComponent<Image>();
        }

        public void OnItemClicked()
        {
            callback(recipe);
        }

        public void Init(Action<BuildingRecipe> callback, BuildingRecipe recipe)
        {
            this.callback = callback;
            this.recipe = recipe;
            this.iconImage.sprite = Register.GetItemByItemId(recipe.FinalItem.ItemId).Sprite();
        }
    }
}