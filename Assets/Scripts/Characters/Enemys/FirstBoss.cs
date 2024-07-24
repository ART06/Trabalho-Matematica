using System.Collections;
using UnityEngine;

public class FirstBoss : Enemy
{
    public bool fBoss;

    protected override void Start()
    {
        base.Start();
        fBoss = true;
    }
    public void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && fBoss) GameManager.instance.isFighting = true;
        if (GameManager.instance.enemyTurn) BossRound();
    }
    public override void BossRound()
    {
        if (GameManager.instance.isFighting && GameManager.instance.enemyTurn)
        {
            base.BossRound();

            if (randomAction <= 5)
            {
                if (enemy != null) anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1.5f);
            }
            else if (randomAction >= 6 && Life.health < Life.maxHealth ||
                randomAction <= 8 && Life.health < Life.maxHealth)
            {
                StartCoroutine(HealEvent());
            }
            else if (randomAction >= 9)
            {
                if (enemy != null) anim.SetTrigger("Crit Atk");
                if (enemy != null) critAnim.SetTrigger("Crit");
                StartCoroutine(nameof(EnemyDoubleDmg));
                Invoke("ActiveHabPanel", 1.5f);
            }
            else
            {
                GameManager.instance.enemyTurn = true;
                enemy.BossRound();
            }
        }
    }
    public IEnumerator HealEvent()
    {
        if (enemy != null) specAnim.SetTrigger("Heal");
        yield return new WaitForSeconds(0.5f);
        Life.GetHeal(Life.healValue);
        Invoke("ActiveHabPanel", 1.5f);
    }
    public override void Death()
    {
        base.Death();
        fBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}