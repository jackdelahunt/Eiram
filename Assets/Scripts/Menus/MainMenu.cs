using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject NewMenu = null;
        [SerializeField] private GameObject LoadMenu = null;
        [SerializeField] private GameObject OptionsMenu = null;

        public void NewClicked()
        {
            NewMenu.SetActive(true);
            LoadMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
        
        public void LoadClicked()
        {
            NewMenu.SetActive(false);
            LoadMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OptionsClicked()
        {
            NewMenu.SetActive(false);
            LoadMenu.SetActive(false);
            OptionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        
        public void QuitClicked()
        {
            Application.Quit();
        }
    }
}
