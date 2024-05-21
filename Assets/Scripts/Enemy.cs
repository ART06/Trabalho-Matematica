using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeCtrl))]
public class Enemy : Character
{
    protected InputHandler inputHandler;
    public bool isFightingPlayer;
    protected void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && !Life.dead) isFightingPlayer = true;
        else isFightingPlayer = false;
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
    protected void DealDmg()
    {
        if (inputHandler.monsterDealDmg)
        {
            player.TakeDmg(atqDmg);
        }
    }
    public override void Death()
    {
        base.Death();

        //enemy.anim.GetBool("Death");
        
        Invoke(nameof(Deactivate), 0.5f);
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);
        //enemy.anim.SetTrigger("Hurt");
        //ELifeCtrlSoundManager.PlaySFX(ELifeCtrlSoundManager.enemyHit);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}