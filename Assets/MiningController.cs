﻿using UnityEngine;
using MR.Changes;

public class MiningController : MonoBehaviour
{
    public enum MiningState
    {
        None,
        Cutting
    }

    public MiningState currentState;
    public float timer = 0;
    public bool hit = false;
    GameObject other;

    public void SetMiningState(MiningState state)
    {
        if (currentState != state)
        {
            timer = Time.realtimeSinceStartup;
        }
        currentState = state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        timer = Time.realtimeSinceStartup;
        other = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hit = false;
        timer = 0;
    }

    public void Update()
    {
        if (hit && timer != 0)
        {
            if (Time.realtimeSinceStartup - timer > 3f && 
                currentState == MiningState.Cutting && 
                other.CompareTag("tree"))
            {
                timer = 0;
                Vector2 location = other.transform.position;
                Change change = new TreeChopped(location, Time.time);
                other.transform.parent.parent.GetComponent<Forest>().AddChange(change);
            }
        }
    }
}
