using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenu = null;
        [SerializeField] private GameObject StartMenu = null;
        [SerializeField] private TMP_Dropdown ResolutionDropdown = null;
        [SerializeField] private TMP_Dropdown ScreenOptionsDropdown = null;

        public void OnEnable()
        {
            ResolutionDropdown.options.Clear();
            for(int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + " x " + resolution.height));
                
                if (resolution.Equals(Screen.currentResolution)) // set current resolution
                {
                    ResolutionDropdown.value = i;
                }
            }

            ScreenOptionsDropdown.value = (int)Screen.fullScreenMode;

        }
        
        public void ApplyClicked()
        {
            var selectedResolution = Screen.resolutions[ResolutionDropdown.value];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, (FullScreenMode)ScreenOptionsDropdown.value);
        }

        public void BackClicked()
        {
            MainMenu.SetActive(true);   
            StartMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
