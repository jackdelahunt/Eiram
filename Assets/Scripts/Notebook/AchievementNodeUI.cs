using System.Collections;
using System.Collections.Generic;
using Notebook;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Notebook
{
    public class AchievementNodeUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text title = null;
        [SerializeField] private Image thumbnail = null;
        [SerializeField] private TMP_Text description = null;
        private AchievementNode node;

        public void Init(AchievementNode node)
        {
            this.node = node;
            title.text = this.node.title;
            thumbnail.sprite = this.node.thumbnail;
            description.text = this.node.description;
        }
    }
}
