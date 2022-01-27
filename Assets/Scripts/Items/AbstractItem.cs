using System;
using Eiram;
using Items.Items;
using Players;
using Tags;
using UnityEngine;

namespace Items
{
    public class AbstractItem
    {
        protected ConcreteItemData concreteItemData;
        protected ItemStack defaultItemStack;
        
        public AbstractItem(ConcreteItemData concreteItemData)
        {
            this.concreteItemData = concreteItemData;
            defaultItemStack = new ItemStack(concreteItemData.itemId, 0);
        }

        public virtual void OnBreak(Vector3Int worldPosition, ItemStack stack, Player player) {}

        public virtual bool OnUse(Vector3Int worldPosition, ItemStack stack, Player player)
        {
            return false;
        }
        
        public string ItemName()
        {
            return concreteItemData.itemName;
        }

        public ItemId ItemId()
        {
            return concreteItemData.itemId;
        }
        
        public TileId TileId()
        {
            return concreteItemData.tileId;
        }
        
        public Sprite Sprite()
        {
            return concreteItemData.sprite;
        }
        
        public int MaxStack()
        {
            return concreteItemData.maxStack;
        }


        public bool IsToolItem(out ToolItemData itemData)
        {
            if (concreteItemData is ToolItemData data)
            {
                itemData = data;
                return true;
            }

            itemData = null;
            return false;
        }

        public ItemStack DefaultItemStack() => defaultItemStack.Clone() as ItemStack;
    }

    public class DirtItem : AbstractItem
    {
        public DirtItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class GrassItem : AbstractItem
    {
        public GrassItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class ThornsItem : AbstractItem
    {
        public ThornsItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class TrellisItem : AbstractItem
    {
        public TrellisItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class ChestItem : AbstractItem
    {
        public ChestItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class StickItem : AbstractItem
    {
        public StickItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
    
    public class WoodClumpItem : AbstractItem
    {
        public WoodClumpItem(ConcreteItemData concreteItemData) : base(concreteItemData) {}
    }
}