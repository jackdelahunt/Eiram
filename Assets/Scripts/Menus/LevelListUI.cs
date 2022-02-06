using System;
using System.Collections.Generic;
using IO;
using UnityEngine;

namespace Menus
{
    public class LevelListUI : MonoBehaviour
    {
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject levelSelectPrefab;
        
        public void OnEnable()
        {
            var childList = new List<Transform>();
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                childList.Add(contentTransform.GetChild(i));
            }
            
            childList.ForEach(t => Destroy(t.gameObject));

            var saves = Filesystem.AllSaves();
            saves.ForEach(save =>
            {
                var go = Instantiate(levelSelectPrefab, contentTransform);
                go.GetComponent<LevelSelect>().Init(save);
            });
        }
    }
}