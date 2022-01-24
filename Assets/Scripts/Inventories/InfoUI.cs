using System;
using System.Collections;
using Events;
using Registers;
using Tiles;
using TMPro;
using UnityEngine;

namespace Inventories
{
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private GameObject infoPanel;

        private Coroutine infoPanelReset;

        public void Awake()
        {
            EiramEvents.TileInfoRequestEvent += OnTileInfoRequest;
        }

        public void Start()
        {
            infoPanelReset = StartCoroutine(nameof(InfoPanelDead));
        }

        public void OnDestroy()
        {
            EiramEvents.TileInfoRequestEvent -= OnTileInfoRequest;
        }

        private void OnTileInfoRequest(SerialTileData tileData)
        {
            StopCoroutine(infoPanelReset);
            infoPanelReset = StartCoroutine(nameof(InfoPanelDead));
            infoPanel.SetActive(true);
            title.text = Register.GetTileByTileId(tileData.TileId).TileName();
        }

        private IEnumerator InfoPanelDead()
        {
            yield return new WaitForSeconds(3.0f);
            infoPanel.SetActive(false);
        }
    }
}