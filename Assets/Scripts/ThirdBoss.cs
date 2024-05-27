using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBoss : Enemy
{
    public bool thirdBossClear;

    protected void Start()
    {
        thirdBossClear = false;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && !thirdBossClear) isFightingPlayer = true;
    }
    public override void Death()
    {
        base.Death();
        thirdBossClear = true;
    }
}