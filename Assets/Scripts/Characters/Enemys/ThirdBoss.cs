using System.Collections;
using UnityEngine;

public class ThirdBoss : Enemy
{
    public bool thirdBoss;
    public bool alreadyFreezed;

    protected override void Start()
    {
        base.Start();
        thirdBoss = true;
        alreadyFreezed = false;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && thirdBoss) GameManager.instance.isFighting = true;
        if (GameManager.instance.enemyTurn) BossRound();
    }
    public override void BossRound()
    {
        if (GameManager.instance.isFighting && GameManager.instance.enemyTurn)
        {
            base.BossRound();
            
            if (randomAction <= 6)
            {
                alreadyFreezed = false;
                player.isFreeze = false;
                if (enemy != null) anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1.5f);
            }
            else if (randomAction == 7 && !alreadyFreezed || randomAction == 8 && !alreadyFreezed)
            {
                StartCoroutine(nameof(FreezeEvent));
            }
            else if (randomAction >= 9)
            {
                alreadyFreezed = false;
                player.isFreeze = false;
                if (enemy != null) anim.SetTrigger("Crit Atk");
                Invoke("ActiveHabPanel", 1.5f);
            }
            else
            {
                GameManager.instance.enemyTurn = true;
                enemy.BossRound();  
            }
        }
    }
    public IEnumerator FreezeEvent()
    {
        if (enemy != null) anim.SetTrigger("Spec");
        yield return new WaitForSeconds(0.5f);
        alreadyFreezed = true;
        player.isFreeze = true;
        yield return new WaitForSeconds(2f);
        GameManager.instance.enemyTurn = true;
        enemy.BossRound();
    }
    public override void Death()
    {
        base.Death();
        thirdBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}