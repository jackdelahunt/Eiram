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
        [SerializeField] private TMP_Text description;
        [SerializeField] private GameObject infoPanel;

        private bool dontCloseInfo = false;
        
        public void Awake()
        {
            EiramEvents.TileInfoRequestEvent += OnTileInfoRequest;
        }

        public void Start()
        {
            InvokeRepeating(nameof(InfoPanelDead), 0.0f, 0.5f);
        }

        public void OnDestroy()
        {
            EiramEvents.TileInfoRequestEvent -= OnTileInfoRequest;
        }

        private void OnTileInfoRequest(SerialTileData tileData)
        {
            dontCloseInfo = true;
            infoPanel.SetActive(true);
            var tile = Register.GetTileByTileId(tileData.TileId);
            title.text = tile.TileName();
            description.text = $"ID: {(int)tile.TileId()}\nTool: {tile.RequiredToolType()}\nLevel: {tile.RequiredToolLevel()}";
        }

        private void InfoPanelDead()
        {
            if (dontCloseInfo)
            {
                dontCloseInfo = false;
                return;
            }
            
            infoPanel.SetActive(false);
        }
    }
}