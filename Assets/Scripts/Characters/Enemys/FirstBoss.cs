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
    public void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && firstBoss) GameManager.instance.isFighting = true;
        if (GameManager.instance.enemyTurn) BossRound();
    }
    public override void BossRound()
    {
        if (GameManager.instance.isFighting && GameManager.instance.enemyTurn)
        {
            base.BossRound();
            
            if (randomAction <= 5)
            {
                if (enemy != null) enemy.anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction == 6 || randomAction == 7)
            {
                if (enemy != null) enemy.anim.SetTrigger("Crit Atk");
                if (enemy != null) critAnim.SetTrigger("Crit");
                StartCoroutine(nameof(EnemyDoubleDmg));
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction >= 8 && Life.health < Life.maxHealth && !specIsOnCooldown)
            {
                StartCoroutine(HealEvent());
                remainCooldown = roundCooldown;
                specIsOnCooldown = true;
                if (cooldownPanel != null) cooldownPanel.SetActive(true);
            }
            else BossRound();
        }
        GameManager.instance.enemyTurn = false;
    }
    public IEnumerator HealEvent()
    {
        if (enemy != null) specAnim.SetTrigger("Heal");
        Invoke("ActiveHabPanel", 1f);
        yield return new WaitForSeconds(0.5f);
        Life.GetHeal(Life.healValue);
    }
    public override void Death()
    {
        base.Death();
        firstBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}