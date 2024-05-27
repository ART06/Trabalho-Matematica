using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Enemy
{
    public bool firstBossClear;

    protected void Start()
    {
        firstBossClear = false;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && !firstBossClear) isFightingPlayer = true;
    }
    public override void Death()
    {
        base.Death();
        firstBossClear = true;
        isFightingPlayer = false;
    }
}