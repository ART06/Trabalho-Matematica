using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Enemy
{
    public bool secondBossClear;

    protected void Start()
    {
        secondBossClear = false;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && !secondBossClear) isFightingPlayer = true;
    }
    public override void Death()
    {
        base.Death();
        secondBossClear = true;
    }
}