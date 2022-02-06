using System;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject continueGameObject;
        [SerializeField] private GameObject saveGameObject;
        [SerializeField] private GameObject quitGameObject;

        public void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                continueGameObject.SetActive(true);
                saveGameObject.SetActive(true);
                quitGameObject.SetActive(true);
            }
        }

        public void ContinueClicked()
        {
            continueGameObject.SetActive(false);
            saveGameObject.SetActive(false);
            quitGameObject.SetActive(false);
        }
        
        public void SaveClicked()
        {
            EiramEvents.OnSaveToDiskRequestEvent();
        }
        
        public void QuitClicked()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}