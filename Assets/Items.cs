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
            Dialogue.Create()
                .Sentence("You founds " + GetType().ToString().Replace("MR.Items.", "").ToLower() + "!")
                .Show();
        }
    }

    public class Axe : Item
    {

    }

    public class Apple : Item
    {

    }
}
