using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class NewMenu : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenu = null;
        [SerializeField] private TMP_Text NameInput = null;
        [SerializeField] private TMP_Text SeedInput = null;

        public void CreateClicked()
        {
            PlayerPrefs.SetString("save_name", NameInput.text);
            PlayerPrefs.SetInt("seed", SeedInput.text.GetHashCode() / SeedInput.text.Length);
            SceneManager.LoadScene("LoadingScreen");
        }

        public void BackClicked()
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}