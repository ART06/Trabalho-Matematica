using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Enemy
{
    public bool firstBoss;

    private void Start()
    {
        firstBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && firstBoss) GameManager.instance.isFighting = true;
    }

    public override void Death()
    {
        base.Death();
        firstBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}