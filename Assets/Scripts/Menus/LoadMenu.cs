using UnityEngine;

namespace Menus
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenu = null;

        public void BackClicked()
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}