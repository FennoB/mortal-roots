using System;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Items
{
    public class Item
    {
        private static HashSet<Type> firstPickup = new HashSet<Type>();
        private Player player;

        public virtual void PickedUp(Player p)
        {
            player = p;
            if (!firstPickup.Contains(GetType()))
            {
                Tutorial();
            }
            firstPickup.Add(GetType());
        }
        public virtual void Dropped()
        {
            player = null;
        }
        public virtual void Used()
        {
        }
        public virtual void Tutorial()
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
        }
    }

    public class Axe : Item
    {
        public override void Tutorial()
        {
            Dialogue.Create()
                .Sentence("You found an axe!")
                .Sentence("Press space to use it!")
                .Show();
        }
    }

    public class Apple : Item
    {

    }
}
