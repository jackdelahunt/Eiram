using System;
using Eiram;
using Registers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories
{
    public class BuildingItemUI : MonoBehaviour
    {
        private Image iconImage;
        private Action<ItemId> callback;
        private ItemId id;

        public void Awake()
        {
            iconImage = GetComponent<Image>();
        }

        public void OnItemClicked()
        {
            callback(id);
        }

        public void Init(Action<ItemId> callback, ItemId id)
        {
            this.callback = callback;
            this.id = id;
            this.iconImage.sprite = Register.GetItemByItemId(id).sprite;
        }
    }
}