using IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] private TMP_Text saveName;
        [SerializeField] private TMP_Text lastPlayed;

        private EiramDirectory eiramDirectory = null;
        
        public void OnClick()
        {
            PlayerPrefs.SetString("save_name", eiramDirectory.Name());
            SceneManager.LoadScene("LoadingScreen");
        }
        
        public void Init(EiramDirectory eiramDirectory)
        {
            this.eiramDirectory = eiramDirectory;
            saveName.text = eiramDirectory.Name();

            var lastPlayedDate = eiramDirectory.LastWriteTime();
            lastPlayed.text = $"{lastPlayedDate.Day}/{lastPlayedDate.Month}/{lastPlayedDate.Year}";
        }
    }
}