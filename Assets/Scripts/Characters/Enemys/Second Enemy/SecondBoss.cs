using System.Collections;
using UnityEngine;

public class SecondBoss : Enemy
{
    public bool secondBoss;

    protected override void Start()
    {
        base.Start();
        secondBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && secondBoss)
        {
            GameManager.instance.isFighting = true;
            Life.shieldBar.gameObject.SetActive(true);
        }
            if (GameManager.instance.enemyTurn) BossRound();
        if (enemy != null) ShieldEvent();
    }
    public void BossRound()
    {
        if (GameManager.instance.isFighting && GameManager.instance.enemyTurn)
        {
            randomAction = Random.Range(0, 10);
            GameManager.instance.enemyTurn = false;

            if (randomAction <= 4)
            {
                if (enemy != null) enemy.anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction >= 5 && randomAction <= 7)
            {
                if (enemy != null) enemy.anim.SetTrigger("Crit Atk");
                if (enemy != null) critAnim.SetTrigger("Crit");
                StartCoroutine(nameof(EnemyDoubleDmg));
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction >= 8 && !specIsOnCooldown)
            {
                if (enemy != null) specAnim.SetBool("Is Shield", true);
                Life.GetHeal(Life.shieldValue);
            }
            else BossRound();
        }
        GameManager.instance.enemyTurn = false;
    }
    public void ShieldEvent()
    {
        if (Life.shield <= 0)
            if (enemy != null) specAnim.SetBool("Is Shield", true);
            if (enemy != null) specAnim.SetTrigger("Shield Break");
    }
    public override void Death()
    {
        base.Death();
        secondBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);
        if (Life.health <= 0) Life.shieldBar.gameObject.SetActive(false);
    }
}