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
            
            if (randomAction <= 5)
            {
                alreadyFreezed = false;
                player.isFreeze = false;
                if (enemy != null) anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1.5f);
            }
            else if (randomAction >= 6 && !alreadyFreezed || randomAction <= 8 && !alreadyFreezed)
            {
                alreadyFreezed = true;
                player.isFreeze = true;
                if (enemy != null) anim.SetTrigger("Spec");
                Invoke(nameof(enemy.BossRound), 3f);
                GameManager.instance.enemyTurn = true;
            }
            else if (randomAction >=9)
            {
                alreadyFreezed = false;
                player.isFreeze = false;
                StartCoroutine(nameof(CritEvent));
            }
            else
            {
                Debug.Log("reroll");
                GameManager.instance.enemyTurn = true;
                enemy.BossRound();  
            }
        }
    }
    public IEnumerator CritEvent()
    {
        if (enemy != null) anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        if (enemy != null) anim.SetTrigger("Crit Atk");
        Invoke("ActiveHabPanel", 1.5f);
    }
    public override void Death()
    {
        base.Death();
        thirdBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}