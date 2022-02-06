using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    public class LevelListUI : MonoBehaviour
    {
        [SerializeField] private Transform contentTransform;
        
        public void OnEnable()
        {
            var childList = new List<Transform>();
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                childList.Add(contentTransform.GetChild(i));
            }
            
            childList.ForEach(t => Destroy(t.gameObject));y
            
            Debug.Log("Enable");
            // create new levelselect for each save
        }
    }
}