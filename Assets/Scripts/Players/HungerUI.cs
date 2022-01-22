using System;
using Events;
using TMPro;
using UnityEngine;

namespace Players
{
    public class HungerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText = null;

        public void Awake()
        {
            EiramEvents.PlayerChangedHungerEvent += OnPlayerChangedHunger;
        }

        public void OnDestroy()
        {
            EiramEvents.PlayerChangedHungerEvent -= OnPlayerChangedHunger;
        }

        private void OnPlayerChangedHunger(int hunger)
        {
            countText.text = hunger.ToString();
        }
    }
}