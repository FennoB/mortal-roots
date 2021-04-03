using System;
using System.Collections.Generic;
using UnityEngine;
using MR.Items;

public class GroundItem : MonoBehaviour
{
    Player player;
    public Item item;
    public Sprite[] sprites;
    public string type;

    // Start is called before the first frame update
    void Start()
    {
        if (type == "Axe")
        {
            item = new Axe();
        }
        else if (type == "Apple")
        {
            item = new Apple();
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.groundItem = this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        player.groundItem = this;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.groundItem = null;
    }
}
