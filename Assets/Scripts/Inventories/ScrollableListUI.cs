using UnityEngine;

namespace Inventories
{
    public class ScrollableListUI : MonoBehaviour
    {
        [SerializeField] private RectTransform contentTransform = null;

        
        public GameObject Add(GameObject prefab)
        {
            return Instantiate(prefab, contentTransform);
        }
    }
}