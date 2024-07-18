using System.Collections;
using UnityEngine;

public class SecondBoss : Enemy
{
    public bool secondBoss;

    protected override void Start()
    {
        base.Start();
        secondBoss = true;
        Life.shieldBar.fillAmount = 0;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && secondBoss)
        {
            GameManager.instance.isFighting = true;
            Life.shieldBar.gameObject.SetActive(true);
        }
        else Life.shieldBar.gameObject.SetActive(false);
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
                StartCoroutine(nameof(CritEvent));
            }
            else if (randomAction >= 8 && !specIsOnCooldown)
            {
                if (enemy != null) specAnim.SetBool("Is Shield", true);
                Life.GetHeal(Life.shieldValue);
                Life.isShielded = true;
            }
            else BossRound();
        }
        GameManager.instance.enemyTurn = false;
    }
    public IEnumerator CritEvent()
    {
        if (enemy != null) enemy.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        if (enemy != null) enemy.anim.SetTrigger("Attack");
        Invoke("ActiveHabPanel", 1.5f);
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);

        if (Life.shield <= 0 && Life.isShielded)
        {
            if (enemy != null) specAnim.SetBool("Is Shield", true);
            if (enemy != null) specAnim.SetTrigger("Shield Break");
            Life.isShielded = false;
        }
    }
    public override void Death()
    {
        base.Death();
        secondBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}