using IO;
using TMPro;
using UnityEngine;

namespace Menus
{
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] private TMP_Text saveName;
        [SerializeField] private TMP_Text lastPlayed;
        
        public void OnClick()
        {
            Debug.Log("CLICKED");
        }
        
        public void Init(EiramDirectory eiramDirectory)
        {
            saveName.text = eiramDirectory.Name();

            var lastPlayedDate = eiramDirectory.LastWriteTime();
            lastPlayed.text = $"{lastPlayedDate.Day}/{lastPlayedDate.Month}/{lastPlayedDate.Year}";
        }
    }
}