using UnityEngine;

namespace MR.Changes
{
    public class Change
    {
        Vector2 where;
        float when;

        public Change(Vector2 where, float when)
        {
            this.when = when;
            this.where = where;
        }

        public Vector2 Where { get => where; set => where = value; }
        public float When { get => when; set => when = value; }
    }

    public class TreeChopped : Change
    {
        public TreeChopped(Vector2 where, float when) : base(where, when)
        {
        }
    }
}