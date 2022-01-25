using System;
using System.Collections;
using Eiram;
using Events;
using Registers;
using Tiles;
using TMPro;
using UnityEngine;

namespace Inventories
{
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text tileTitle;
        [SerializeField] private TMP_Text tileDescription;
        
        [SerializeField] private TMP_Text itemTitle;
        [SerializeField] private TMP_Text itemDescription;

        [SerializeField] private GameObject tileInfoPanel;
        [SerializeField] private GameObject itemInfoPanel;

        private bool dontCloseTileInfo = false;
        private bool dontCloseItemInfo = false;
        
        public void Awake()
        {
            EiramEvents.TileInfoRequestEvent += OnTileInfoRequest;
            EiramEvents.ItemInfoRequestEvent += OnItemInfoRequest;
        }

        public void Start()
        {
            InvokeRepeating(nameof(InfoPanelsDead), 0.0f, 0.5f);
        }

        public void OnDestroy()
        {
            EiramEvents.TileInfoRequestEvent -= OnTileInfoRequest;
            EiramEvents.ItemInfoRequestEvent -= OnItemInfoRequest;
        }

        private void OnTileInfoRequest(SerialTileData tileData)
        {
            dontCloseTileInfo = true;
            tileInfoPanel.SetActive(true);
            var tile = Register.GetTileByTileId(tileData.TileId);
            tileTitle.text = tile.TileName();
            tileDescription.text = $"ID: {(int)tile.TileId()}\nTool: {tile.RequiredToolType()}\nLevel: {tile.RequiredToolLevel()}";
        }
        
        private void OnItemInfoRequest(ItemId itemId)
        {
            dontCloseItemInfo = true;
            itemInfoPanel.SetActive(true);
            var item = Register.GetItemByItemId(itemId);
            itemTitle.text = item.ItemName();
            itemDescription.text = $"ID: {(int)item.ItemId()}\nMax stack: {item.MaxStack()}\nTile: {item.TileId()}";
        }

        private void InfoPanelsDead()
        {
            if (dontCloseTileInfo)
                dontCloseTileInfo = false;
            else
                tileInfoPanel.SetActive(false);
            
            if (dontCloseItemInfo)
                dontCloseItemInfo = false;
            else
                itemInfoPanel.SetActive(false);
            
        }
    }
}