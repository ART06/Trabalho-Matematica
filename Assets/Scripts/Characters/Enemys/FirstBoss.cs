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
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction == 6 || randomAction == 7)
            {
                if (enemy != null) anim.SetTrigger("Crit Atk");
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
        yield return new WaitForSeconds(0.5f);
        Life.GetHeal(Life.healValue);
        Invoke("ActiveHabPanel", 1f);
    }
    public override void Death()
    {
        base.Death();
        fBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
    public void OnTurnEnd()
    {
        Debug.Log("passou turno");
        if (specIsOnCooldown)
        {
            remainCooldown--;
            if (cooldownText != null) cooldownText.text = remainCooldown.ToString();
            if (remainCooldown <= 0)
            {
                if (enemy != null)
                    enemySkill.SetActive(true);
                if (cooldownPanel != null)
                    cooldownPanel.SetActive(false);
                specIsOnCooldown = false;
            }
        }
        else
        {
            specIsOnCooldown = false;
            remainCooldown = 0;
        }
    }
}