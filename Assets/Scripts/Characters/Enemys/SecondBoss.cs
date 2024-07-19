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

            if (randomAction <= 4)
            {
                Debug.Log("ataque basico");
                if (enemy != null) anim.SetTrigger("Attack");
                Invoke("ActiveHabPanel", 1f);
            }
            else if (randomAction == 5 || randomAction == 6)
            {
                Debug.Log("ataque pesado/duplo");
                StartCoroutine(nameof(CritEvent));
            }
            else if (randomAction >= 7 && !specIsOnCooldown)
            {
                StartCoroutine(ShieldHeal());
                remainCooldown = roundCooldown;
                specIsOnCooldown = true;
                if (cooldownPanel != null) cooldownPanel.SetActive(true);
            }
            else BossRound();
        }
        GameManager.instance.enemyTurn = false;
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