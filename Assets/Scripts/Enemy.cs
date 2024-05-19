using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeCtrl))]
public class Enemy : MonoBehaviour
{
    protected Player player;
    public int fightRange;
    public bool isFightingPlayer;
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= fightRange) isFightingPlayer = true;
        else isFightingPlayer = false;
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
}