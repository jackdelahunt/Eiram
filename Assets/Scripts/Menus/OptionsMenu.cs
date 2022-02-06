using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenu = null;
        [SerializeField] private GameObject StartMenu = null;

        public void BackClicked()
        {
            MainMenu.SetActive(true);
            StartMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
