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
            foreach (var resolution in Screen.resolutions)
            {
                ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + " x " + resolution.height));
            }
            
        }
        
        public void ApplyClicked()
        {
            var selectedResolution = Screen.resolutions[ResolutionDropdown.value];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, 
                ScreenOptionsDropdown.value switch
                {
                    0 => FullScreenMode.ExclusiveFullScreen,
                    1 => FullScreenMode.FullScreenWindow,
                    2 => FullScreenMode.Windowed,
                    _ => FullScreenMode.ExclusiveFullScreen
                }
                );
        }

        public void BackClicked()
        {
            MainMenu.SetActive(true);   
            StartMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
