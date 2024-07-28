using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System.Security.Cryptography;

public class Enemy : Character
{
    [Header("UI Elements")]
    protected InputHandler input;
    public Animator critAnim;
    public Animator specAnim;

    [Header("Game State Variables")]
    protected int randomAction;
    public bool alreadyFreezed;

    protected virtual void Start()
    {

    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
    public IEnumerator WaitBossAction()
    {
        GameManager.instance.missChance = Random.Range(0, 4);
        Debug.Log("Chance do erro: " + GameManager.instance.missChance);

        yield return new WaitForSeconds(2);
        if (GameManager.instance.missChance == 0) GameManager.instance.missAtk = true;

        if (GameManager.instance.healSkill.isHeal)
        {
            yield return new WaitForSeconds(2);
            GameManager.instance.enemyTurn = true;
        }
        else
        {
            yield return new WaitForSeconds(1);
            GameManager.instance.enemyTurn = true;
        }
    }
    public virtual void BossRound()
    {
        randomAction = Random.Range(0, 11);
        GameManager.instance.enemyTurn = false;
    }
    public IEnumerator EnemyDoubleDmg()
    {
        enemy.atqDmg *= 2;
        yield return new WaitForSeconds(2);
        enemy.atqDmg /= 2;
    }
    public virtual IEnumerator CritEvent()
    {
        if (enemy != null) anim.SetTrigger("Crit Atk");
        yield return new WaitForSeconds(1f);
        Invoke("ActiveHabPanel", 1f);
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
    public IEnumerator FreezeEvent()
    {
        if (enemy != null) anim.SetTrigger("Freeze");
        yield return new WaitForSeconds(0.5f);
        alreadyFreezed = true;
        player.isFreeze = true;
        yield return new WaitForSeconds(2f);
        GameManager.instance.enemyTurn = true;
        enemy.BossRound();
    }
    public IEnumerator HealEvent()
    {
        if (enemy != null) specAnim.SetTrigger("Heal");
        yield return new WaitForSeconds(0.5f);
        Life.GetHeal(Life.healValue);
        Invoke("ActiveHabPanel", 1.5f);
    }
    public void ActiveHabPanel()
    {
        GameManager.instance.habilityPanel.SetActive(true);
        if (GameManager.instance.character != null) GameManager.instance.playerTurn = true;
        GameManager.instance.missAtk = false;
    }
}