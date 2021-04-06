using System;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Items
{
    public interface IItemContainer
    {
        void Destroy();
    }

    public class Item
    {
        private static HashSet<Type> firstPickup = new HashSet<Type>();
        protected Player player;
        protected IItemContainer container;

        public Item(IItemContainer container)
        {
            this.container = container;
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
        public virtual void Info()
        {
            string itemtext = GetType().ToString().Replace("MR.Items.", "").ToLower();
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
}
