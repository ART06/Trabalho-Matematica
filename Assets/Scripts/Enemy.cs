using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool isFightingPlayer;

    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange) isFightingPlayer = true;
        else isFightingPlayer = false;
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
}