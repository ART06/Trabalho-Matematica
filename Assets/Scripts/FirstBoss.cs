using System.Collections;
using UnityEngine;

public class FirstBoss : Enemy
{
    public bool firstBoss;

    protected override void Start()
    {
        base.Start();
        firstBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && firstBoss) GameManager.instance.isFighting = true;

        if (enemyTurn)
        {
            BossRound();
            enemyTurn = false;
        }
    }
    public override void BossRound()
    {
        base.BossRound();
        if (GameManager.instance.isFighting && enemyTurn)
        {
            if (randomAction == 1)
            {
                input.monsterDealDmg = false;
                if (enemy != null) enemy.anim.SetTrigger("Attack");
                StartCoroutine(nameof(input.ActiveHabPanel));
            }
            else if (randomAction == 2)
            {
                input.monsterDealDmg = false;
                if (enemy != null) enemy.anim.SetTrigger("Crit Atk");
                if (enemy != null) critAnim.SetTrigger("Crit");
                StartCoroutine(nameof(input.ActiveHabPanel));
            }
            else if (randomAction == 3 && Life.health < Life.maxHealth)
            {
                input.monsterDealDmg = false;
                StartCoroutine(nameof(HealEvent));
            }
            else BossRound();
        }
    }
    public IEnumerator HealEvent()
    {
        if (enemy != null) specAnim.SetTrigger("Heal");
        StartCoroutine(nameof(input.ActiveHabPanel));
        yield return new WaitForSeconds(1.5f);
        player.Life.GetHeal(player.Life.healValue);
    }
    public override void Death()
    {
        base.Death();
        firstBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}