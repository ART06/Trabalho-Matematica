using System.Collections;
using UnityEngine;

public class SecondBoss : Enemy
{
    public bool sBoss;

    protected override void Start()
    {
        base.Start();
        sBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && sBoss) GameManager.instance.isFighting = true;
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
            else if (randomAction >= 6 && Life.shield < Life.maxHealth ||
                randomAction <= 8 && Life.shield < Life.maxHealth)
            {
                StartCoroutine(ShieldHeal());
            }
            else if (randomAction >= 9)
            {
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
    public IEnumerator ShieldHeal()
    {
        Life.isShielded = true;
        if (enemy != null) anim.SetTrigger("Spec");
        if (enemy != null) specAnim.SetBool("Is Shield", true);
        yield return new WaitForSeconds(0.5f);
        Life.GetHeal(Life.shieldValue);
        Invoke("ActiveHabPanel", 1f);
    }
    public IEnumerator CritEvent()
    {
        if (enemy != null) anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        if (enemy != null) anim.SetTrigger("Attack");
        Invoke("ActiveHabPanel", 1.5f);
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
        sBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}