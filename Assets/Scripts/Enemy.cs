using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeCtrl))]
public class Enemy : Character
{
    public bool isFightingPlayer;

    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && !Life.dead) isFightingPlayer = true;
        else isFightingPlayer = false;
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
    protected override void DealDmg()
    {
        base.DealDmg();

        if (inputHandler.monsterDealDmg)
        {
            Debug.Log(inputHandler.monsterDealDmg);
            enemy.anim.SetTrigger("Attack");
            inputHandler.monsterDealDmg = false;
        }
    }
    public override void Death()
    {
        base.Death();

        enemy.anim.GetBool("Death");
        isFightingPlayer = false;

        Invoke(nameof(Deactivate), 0.5f);
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);
        enemy.anim.SetTrigger("Hurt");
        //ELifeCtrlSoundManager.PlaySFX(ELifeCtrlSoundManager.enemyHit);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}