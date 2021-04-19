using System;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Items
{
    public enum ItemType
    {
        Axe,
        Apple,
        Log
    }

    public interface IItemContainer
    {
        void Destroy();
    }

    public class Item
    {
        private static HashSet<Type> firstPickup = new HashSet<Type>();
        protected Player player;
        protected IItemContainer container;
        protected ItemType itemType;

        public Item(IItemContainer container)
        {
            itemType = (ItemType)Enum.Parse(typeof(ItemType), GetClassName());
            this.container = container;
        }

        public static Item Create(IItemContainer container, ItemType itemType)
        {
            string s = "MR.Items." + Enum.GetName(typeof(ItemType), itemType);
            Type type = Type.GetType(s);
            return (Item)Activator.CreateInstance(type, container);
        }
        public virtual void PickedUp(Player p)
        {
            player = p;
            if (!firstPickup.Contains(GetType()))
            {
                Info();
            }
        }
        public virtual void Dropped()
        {
            player = null;
        }
        public virtual void Used()
        {
        }
        public string GetClassName()
        {
            return GetType().ToString().Replace("MR.Items.", "");
        }
        public ItemType GetItemType()
        {
            return itemType;
        }
        public virtual void Info()
        {
            string itemtext = GetClassName().ToLower();
            if (itemtext.StartsWith("a") ||
                itemtext.StartsWith("e") ||
                itemtext.StartsWith("i") ||
                itemtext.StartsWith("o") ||
                itemtext.StartsWith("u"))
            {
                itemtext = "an " + itemtext;
            }
            else
            {
                itemtext = "a " + itemtext;
            }
            Dialogue.Create()
                .Sentence("You found " + itemtext + "!")
                .Show();
            firstPickup.Add(GetType());
        }
    }

    public class Axe : Item
    {
        public Axe(IItemContainer container) : base(container)
        {
        }

        public override void Info()
        {
            Dialogue.Create()
                .Sentence("You found an axe!")
                .Sentence("Press space to use it!")
                .Show();
        }

        public override void Used()
        {
            player.mining.SetMiningState(MiningController.MiningState.Cutting);
        }
    }

    public class Apple : Item
    {
        public Apple(IItemContainer container) : base(container)
        {
        }

        public override void Used()
        {
            Dialogue.Create()
                .Sentence("Nom nom nom...")
                .Show();
            player.health.hunger = Mathf.Max(0, player.health.hunger - .25f);
            container.Destroy();
        }
    }

    public class Log : Item
    {
        public Log(IItemContainer container) : base(container)
        {
        }
    }
}
