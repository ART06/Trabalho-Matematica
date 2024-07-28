using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThirdBoss : Enemy
{
    public bool thirdBoss;

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
            else if (randomAction >= 9 && Life.shield < Life.maxHealth)
            {
                alreadyFreezed = false;
                player.isFreeze = false;
                StartCoroutine(ShieldHeal());
            }
            else
            {
                GameManager.instance.enemyTurn = true;
                enemy.BossRound();  
            }
        }
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);

        if (Life.shield <= 0 && Life.isShielded)
        {
            if (enemy != null) specAnim.SetBool("Is Shield", false);
            if (enemy != null) specAnim.SetTrigger("Shield Break");
            Life.isShielded = false;
        }
    }
    public override void Death()
    {
        base.Death();
        thirdBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}