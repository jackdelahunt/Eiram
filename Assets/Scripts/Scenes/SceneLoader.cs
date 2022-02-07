using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private TMP_Text percentageText;

        private AsyncOperation loadingOperation = null;
        
        public void Start()
        {
            loadingOperation = SceneManager.LoadSceneAsync("Main");
            loadingOperation.completed += OnSceneLoaded;
        }

        public void Update()
        {
            if (loadingOperation != null)
            {
                percentageText.text = loadingOperation.progress * 100 + "%";
            }
        }

        public void OnSceneLoaded(AsyncOperation operation)
        {

        }
    }
}