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
                                                        description = "Create a thorn bush with wood clumps and sticks to get renewable sticks. Use Organic mass to create tilled soil to plant on. Continue to use organic mass to keep the farm alive",
                                                        status = AchievementStatus.LOCKED,
                                                        requirements = new [] {
                                                            new ItemCountPair() { ItemId = ItemId.THORNS, Amount = 1},
                                                        },
                                                        rewards = new [] {
                                                            new ItemCountPair() { ItemId = ItemId.WOOD_CLUMP, Amount = 3},
                                                            new ItemCountPair() { ItemId = ItemId.DIRT, Amount = 10},
                                                        },
                                                        children = new Achievement[]
                                                        {
                                                            new Achievement()
                                                            {
                                                                parent = null,
                                                                thumbnail = Register.GetItemByItemId(ItemId.ORGANIC_MASS).Sprite(),
                                                                title = "Vegan Meal Deal",
                                                                description = "Gather some organic mass by harvesting natural items like dirt and grass, to start your first farm. Use more organic mass on tilled soil to keep your crops from uprooting",
                                                                status = AchievementStatus.LOCKED,
                                                                requirements = new [] {
                                                                    new ItemCountPair() { ItemId = ItemId.ORGANIC_MASS, Amount = 10},
                                                                },
                                                                rewards = new [] {
                                                                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 3},
                                                                    new ItemCountPair() { ItemId = ItemId.ORGANIC_MASS, Amount = 3},
                                                                    new ItemCountPair() { ItemId = ItemId.THORNS, Amount = 1},
                                                                },
                                                                children = new Achievement[]
                                                                {
                                                                    new Achievement()
                                                                    {
                                                                        parent = null,
                                                                        thumbnail = Register.GetItemByItemId(ItemId.MINI_TREE).Sprite(),
                                                                        title = "I Am Groot",
                                                                        description = "Combine the some organic mass you have collected and your thorn bush to create a small tree for farming.",
                                                                        status = AchievementStatus.LOCKED,
                                                                        requirements = new [] {
                                                                            new ItemCountPair() { ItemId = ItemId.MINI_TREE, Amount = 1},
                                                                        },
                                                                        rewards = new [] {
                                                                            new ItemCountPair() { ItemId = ItemId.STICK, Amount = 5},
                                                                            new ItemCountPair() { ItemId = ItemId.WOOD, Amount = 5},
                                                                        },
                                                                        children = new Achievement[]
                                                                        {
                                                                            new Achievement()
                                                                            {
                                                                                parent = null,
                                                                                thumbnail = Register.GetItemByItemId(ItemId.STONE).Sprite(),
                                                                                title = "Stoned",
                                                                                description = "Use a pickaxe to break some stone, this can be used instead of wood for better tools",
                                                                                status = AchievementStatus.LOCKED,
                                                                                requirements = new [] {
                                                                                    new ItemCountPair() { ItemId = ItemId.STONE, Amount = 10},
                                                                                },
                                                                                rewards = new [] {
                                                                                    new ItemCountPair() { ItemId = ItemId.STONE, Amount = 5},
                                                                                },
                                                                                children = new Achievement[]
                                                                                {
                                                                                    new Achievement()
                                                                                    {
                                                                                        parent = null,
                                                                                        thumbnail = Register.GetItemByItemId(ItemId.STONE_SHOVEL).Sprite(),
                                                                                        title = "Stone Shovel",
                                                                                        description = "Use sticks and stone to make a shovel by going to your inventory",
                                                                                        status = AchievementStatus.LOCKED,
                                                                                        requirements = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE_SHOVEL, Amount = 1},
                                                                                        },
                                                                                        rewards = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE, Amount = 3},
                                                                                        },
                                                                                        children = new Achievement[]
                                                                                            {}
                                                                                    },
                                                                                    new Achievement()
                                                                                    {
                                                                                        parent = null,
                                                                                        thumbnail = Register.GetItemByItemId(ItemId.STONE_PICKAXE).Sprite(),
                                                                                        title = "Stone Pickaxe",
                                                                                        description = "Use sticks and stone scrap to make a pickaxe by going to your inventory",
                                                                                        status = AchievementStatus.LOCKED,
                                                                                        requirements = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE_PICKAXE, Amount = 1},
                                                                                        },
                                                                                        rewards = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE, Amount = 3},
                                                                                        },
                                                                                        children = new Achievement[]
                                                                                        {
                                                                                            new Achievement()
                                                                                            {
                                                                                                parent = null,
                                                                                                thumbnail = Register.GetItemByItemId(ItemId.STONE_SEED).Sprite(),
                                                                                                title = "Auto Stone",
                                                                                                description = "Stone seeds are dropped when mining stone, these can be used to gather stone for you",
                                                                                                status = AchievementStatus.LOCKED,
                                                                                                requirements = new [] {
                                                                                                    new ItemCountPair() { ItemId = ItemId.STONE_SEED, Amount = 1},
                                                                                                },
                                                                                                rewards = new [] {
                                                                                                    new ItemCountPair() { ItemId = ItemId.STONE, Amount = 5},
                                                                                                },
                                                                                                children = new Achievement[]
                                                                                                    {
                                                                                                        new Achievement()
                                                                                                        {
                                                                                                            parent = null,
                                                                                                            thumbnail = Register.GetItemByItemId(ItemId.COPPER_SHOVEL).Sprite(),
                                                                                                            title = "Copper Shovel",
                                                                                                            description = "Use sticks and copper to make a shovel by going to your inventory",
                                                                                                            status = AchievementStatus.LOCKED,
                                                                                                            requirements = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER_SHOVEL, Amount = 1},
                                                                                                            },
                                                                                                            rewards = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER, Amount = 3},
                                                                                                            },
                                                                                                            children = new Achievement[]
                                                                                                                {}
                                                                                                        },
                                                                                                        new Achievement()
                                                                                                        {
                                                                                                            parent = null,
                                                                                                            thumbnail = Register.GetItemByItemId(ItemId.COPPER_PICKAXE).Sprite(),
                                                                                                            title = "Copper Pickaxe",
                                                                                                            description = "Use sticks and copper scrap to make a pickaxe by going to your inventory",
                                                                                                            status = AchievementStatus.LOCKED,
                                                                                                            requirements = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER_PICKAXE, Amount = 1},
                                                                                                            },
                                                                                                            rewards = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER, Amount = 3},
                                                                                                            },
                                                                                                            children = new Achievement[]
                                                                                                            {
                                                                                                                new Achievement()
                                                                                                                {
                                                                                                                    parent = null,
                                                                                                                    thumbnail = Register.GetItemByItemId(ItemId.COPPER_SEED).Sprite(),
                                                                                                                    title = "Auto Copper",
                                                                                                                    description = "Craft copper seeds by using stone and copper",
                                                                                                                    status = AchievementStatus.LOCKED,
                                                                                                                    requirements = new [] {
                                                                                                                        new ItemCountPair() { ItemId = ItemId.COPPER_SEED, Amount = 1},
                                                                                                                    },
                                                                                                                    rewards = new [] {
                                                                                                                        new ItemCountPair() { ItemId = ItemId.COPPER, Amount = 5},
                                                                                                                    },
                                                                                                                    children = new Achievement[]
                                                                                                                        {
                                                                                                                            new Achievement()
                                                                                                                            {
                                                                                                                                parent = null,
                                                                                                                                thumbnail = Register.GetItemByItemId(ItemId.COPPER_SHOVEL).Sprite(),
                                                                                                                                title = "Gold Shovel",
                                                                                                                                description = "Use sticks and gold to make a shovel by going to your inventory",
                                                                                                                                status = AchievementStatus.LOCKED,
                                                                                                                                requirements = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD_SHOVEL, Amount = 1},
                                                                                                                                },
                                                                                                                                rewards = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD, Amount = 3},
                                                                                                                                },
                                                                                                                                children = new Achievement[]
                                                                                                                                    {}
                                                                                                                            },
                                                                                                                            new Achievement()
                                                                                                                            {
                                                                                                                                parent = null,
                                                                                                                                thumbnail = Register.GetItemByItemId(ItemId.GOLD_PICKAXE).Sprite(),
                                                                                                                                title = "Gold Pickaxe",
                                                                                                                                description = "Use sticks and gold scrap to make a pickaxe by going to your inventory",
                                                                                                                                status = AchievementStatus.LOCKED,
                                                                                                                                requirements = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD_PICKAXE, Amount = 1},
                                                                                                                                },
                                                                                                                                rewards = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD, Amount = 3},
                                                                                                                                },
                                                                                                                                children = new Achievement[]
                                                                                                                                {
                                                                                                                                    new Achievement()
                                                                                                                                    {
                                                                                                                                        parent = null,
                                                                                                                                        thumbnail = Register.GetItemByItemId(ItemId.GOLD_SEED).Sprite(),
                                                                                                                                        title = "Auto Gold",
                                                                                                                                        description = "Craft gold seeds by using stone and gold",
                                                                                                                                        status = AchievementStatus.LOCKED,
                                                                                                                                        requirements = new [] {
                                                                                                                                            new ItemCountPair() { ItemId = ItemId.GOLD_SEED, Amount = 1},
                                                                                                                                        },
                                                                                                                                        rewards = new [] {
                                                                                                                                            new ItemCountPair() { ItemId = ItemId.GOLD, Amount = 5},
                                                                                                                                        },
                                                                                                                                        children = new Achievement[]
                                                                                                                                            {}
                                                                                                                                    },
                                                                                                                                }
                                                                                                                            },
                                                                                                                            new Achievement()
                                                                                                                            {
                                                                                                                                parent = null,
                                                                                                                                thumbnail = Register.GetItemByItemId(ItemId.GOLD_AXE).Sprite(),
                                                                                                                                title = "Gold Axe",
                                                                                                                                description = "Use sticks and gold scrap to make an axe by going to your inventory",
                                                                                                                                status = AchievementStatus.LOCKED,
                                                                                                                                requirements = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD_AXE, Amount = 1},
                                                                                                                                },
                                                                                                                                rewards = new [] {
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                                    new ItemCountPair() { ItemId = ItemId.GOLD, Amount = 3},
                                                                                                                                },
                                                                                                                                children = new Achievement[]
                                                                                                                                    {}
                                                                                                                            },
                                                                                                                        }
                                                                                                                },
                                                                                                            }
                                                                                                        },
                                                                                                        new Achievement()
                                                                                                        {
                                                                                                            parent = null,
                                                                                                            thumbnail = Register.GetItemByItemId(ItemId.COPPER_AXE).Sprite(),
                                                                                                            title = "Copper Axe",
                                                                                                            description = "Use sticks and copper scrap to make an axe by going to your inventory",
                                                                                                            status = AchievementStatus.LOCKED,
                                                                                                            requirements = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER_AXE, Amount = 1},
                                                                                                            },
                                                                                                            rewards = new [] {
                                                                                                                new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                                                new ItemCountPair() { ItemId = ItemId.COPPER, Amount = 3},
                                                                                                            },
                                                                                                            children = new Achievement[]
                                                                                                                {}
                                                                                                        },
                                                                                                    }
                                                                                            },
                                                                                        }
                                                                                    },
                                                                                    new Achievement()
                                                                                    {
                                                                                        parent = null,
                                                                                        thumbnail = Register.GetItemByItemId(ItemId.STONE_AXE).Sprite(),
                                                                                        title = "Stone Axe",
                                                                                        description = "Use sticks and stone scrap to make an axe by going to your inventory",
                                                                                        status = AchievementStatus.LOCKED,
                                                                                        requirements = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE_AXE, Amount = 1},
                                                                                        },
                                                                                        rewards = new [] {
                                                                                            new ItemCountPair() { ItemId = ItemId.STICK, Amount = 1},
                                                                                            new ItemCountPair() { ItemId = ItemId.STONE, Amount = 3},
                                                                                        },
                                                                                        children = new Achievement[]
                                                                                            {}
                                                                                    },
                                                                                }
                                                                            },
                                                                        }
                                                                    },
                                                                }
                                                            },
                                                        }
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