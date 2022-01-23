using System;
using System.Collections.Generic;
using Eiram;
using Recipes;
using Registers;
using UnityEngine;

namespace Notebook
{
    public class Achievement
    {
        public Achievement parent;
        public Sprite thumbnail;
        public string title;
        public string description;
        public AchievementStatus status;
        public ItemCountPair[] requirements;
        public ItemCountPair[] rewards;
        public Achievement[] children;

        public static Achievement NewTree()
        {
            var root = new Achievement()
            {
                parent = null,
                thumbnail = Register.GetItemByItemId(ItemId.DIRT).Sprite(),
                title = "Mining Dirt",
                description = "Break dirt with your empty hand",
                status = AchievementStatus.AVAILABLE,
                requirements = new [] {
                    new ItemCountPair() { ItemId = ItemId.GRASS, Amount = 1},
                },
                rewards = new [] {
                    new ItemCountPair() { ItemId = ItemId.DIRT, Amount = 10}
                },
                children = new []
                {
                    new Achievement()
                    {
                        parent = null,
                        thumbnail = Register.GetItemByItemId(ItemId.GRASS).Sprite(),
                        title = "Mining Grass",
                        description = "Break dirt with your empty hand",
                        status = AchievementStatus.LOCKED,
                        requirements = new [] {
                            new ItemCountPair() { ItemId = ItemId.DIRT, Amount = 1},
                        },
                        rewards = new [] {
                            new ItemCountPair() { ItemId = ItemId.GRASS, Amount = 10}
                        },
                        children = new Achievement[] { }
                    }
                }
            };

            return root;
        }

        public List<Achievement> AllAchievements(List<Achievement> achievements = null)
        {
            if (achievements == null) achievements = new List<Achievement>();

            achievements.Add(this);
            foreach (var child in children)
            {
                child.AllAchievements(achievements);
            }
            
            return achievements;
        }

        public AchievementData GetSerializableData()
        {
            return new AchievementData()
            {
                name = this.title,
                status = this.status
            };
        }
        
    }

    [Serializable]
    public class AchievementData
    {
        public string name;
        public AchievementStatus status;
    }

    public enum AchievementStatus
    {
        LOCKED = 0,
        AVAILABLE = 1,
        COMPLETE = 2
    }
}