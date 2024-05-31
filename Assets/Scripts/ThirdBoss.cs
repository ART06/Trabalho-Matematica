using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBoss : Enemy
{
    public bool thirdBoss;

    private void Start()
    {
        thirdBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && thirdBoss) GameManager.instance.isFighting = true;
    }

    public override void Death()
    {
        base.Death();
        thirdBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}