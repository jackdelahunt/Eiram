using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject StartMenu = null;
        [SerializeField] private GameObject OptionsMenu = null;
        
        public void StartClicked()
        {
            StartMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OptionsClicked()
        {
            StartMenu.SetActive(false);
            OptionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        
        public void QuitClicked()
        {
            Application.Quit();
        }
    }
}
