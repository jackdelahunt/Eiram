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
                thumbnail = Register.GetItemByItemId(ItemId.STICK).Sprite(),
                title = "Get a stick",
                description = "Break leaves from a tree with your hand to get some sticks",
                status = AchievementStatus.AVAILABLE,
                requirements = new [] {
                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 4},
                },
                rewards = new [] {
                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 2}
                },
                children = new Achievement[]
                {
                    new Achievement()
                    {
                        parent = null,
                        thumbnail = Register.GetItemByItemId(ItemId.WOOD_CLUMP).Sprite(),
                        title = "Picking up the scraps",
                        description = "Go to a biome with grass and find some scrap on the floor",
                        status = AchievementStatus.LOCKED,
                        requirements = new [] {
                            new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 5},
                        },
                        rewards = new [] {
                            new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 5},
                            new ItemCountPair() { ItemId = ItemId.STICK, Amount = 2},
                        },
                        children = new Achievement[]
                        {
                            new Achievement()
                            {
                                parent = null,
                                thumbnail = Register.GetItemByItemId(ItemId.WOOD_SHOVEL).Sprite(),
                                title = "Wooden Shovel",
                                description = "Use sticks and wood scrap to make a shovel by going to your inventory",
                                status = AchievementStatus.LOCKED,
                                requirements = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_SHOVEL, Amount = 1},
                                },
                                rewards = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 10},
                                },
                                children = new Achievement[]
                                    {}
                            },
                            new Achievement()
                            {
                                parent = null,
                                thumbnail = Register.GetItemByItemId(ItemId.WOOD_AXE).Sprite(),
                                title = "Wooden Axe",
                                description = "Use sticks and wood scrap to make an axe by going to your inventory",
                                status = AchievementStatus.LOCKED,
                                requirements = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_AXE, Amount = 1},
                                },
                                rewards = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 10},
                                },
                                children = new Achievement[]
                                {
                                    new Achievement()
                                    {
                                        parent = null,
                                        thumbnail = Register.GetItemByItemId(ItemId.WOOD).Sprite(),
                                        title = "Break it to pulp",
                                        description = "Use your newly acquired axe to chop down a tree to get some wood",
                                        status = AchievementStatus.LOCKED,
                                        requirements = new [] {
                                            new ItemCountPair() { ItemId = ItemId.WOOD, Amount = 10},
                                        },
                                        rewards = new [] {
                                            new ItemCountPair() { ItemId = ItemId.WOOD_AXE, Amount = 1},
                                        },
                                        children = new Achievement[]
                                        {
                                            new Achievement()
                                            {
                                                parent = null,
                                                thumbnail = Register.GetItemByItemId(ItemId.PLANK).Sprite(),
                                                title = "Do the plank",
                                                description = "Create planks from the wood you have gathered, planks are more efficient replacement for wood clumps",
                                                status = AchievementStatus.LOCKED,
                                                requirements = new [] {
                                                    new ItemCountPair() { ItemId = ItemId.PLANK, Amount = 50},
                                                },
                                                rewards = new [] {
                                                    new ItemCountPair() { ItemId = ItemId.WOOD, Amount = 5},
                                                },
                                                children = new Achievement[]
                                                {
                                                    new Achievement()
                                                    {
                                                        parent = null,
                                                        thumbnail = Register.GetItemByItemId(ItemId.THORNS).Sprite(),
                                                        title = "Getting Prickly",
                                                        description = "Create a thorn bush with wood clumps and sticks to get renewable sticks",
                                                        status = AchievementStatus.LOCKED,
                                                        requirements = new [] {
                                                            new ItemCountPair() { ItemId = ItemId.THORNS, Amount = 1},
                                                        },
                                                        rewards = new [] {
                                                            new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 3},
                                                            new ItemCountPair() { ItemId = ItemId.DIRT, Amount = 10},
                                                        },
                                                        children = new Achievement[]
                                                            {}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Achievement()
                            {
                                parent = null,
                                thumbnail = Register.GetItemByItemId(ItemId.WOOD_PICKAXE).Sprite(),
                                title = "Wooden Pickaxe",
                                description = "Use sticks and wood scrap to make a pickaxe by going to your inventory",
                                status = AchievementStatus.LOCKED,
                                requirements = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_PICKAXE, Amount = 1},
                                },
                                rewards = new [] {
                                    new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 10},
                                },
                                children = new Achievement[]
                                    {}
                            }
                        }
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