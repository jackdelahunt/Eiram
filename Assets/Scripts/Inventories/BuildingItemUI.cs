using System;
using Eiram;
using Recipes;
using Registers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventories
{
    public class BuildingItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image iconImage;
        private Action<BuildingRecipe> callback;
        private BuildingRecipe recipe;
        [SerializeField] private GameObject hoverCard;
        [SerializeField] private GameObject countableItemPrefab = null;
        [SerializeField] private ScrollableListUI requirementsList;

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
            
            foreach (var itemCountPair in recipe.Ingredients)
            {
                var icon = requirementsList.Add(countableItemPrefab);
                var countableItem = icon.GetComponent<CountableItem>();
                countableItem.Image.sprite = Register.GetItemByItemId(itemCountPair.ItemId).Sprite();
                countableItem.Count.text = itemCountPair.Amount.ToString();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverCard.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCard.SetActive(false);
        }
    }
}