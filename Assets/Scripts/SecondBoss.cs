using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Enemy
{
    public bool secondBoss;

    private void Start()
    {
        secondBoss = true;
    }
    protected void FixedUpdate()
    {
        //if (DistanceToPlayer() <= atqRange && secondBoss) GameManager.instance.isFighting = true;
    }

    public override void Death()
    {
        base.Death();
        secondBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}