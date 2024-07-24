using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [Header("UI Elements")]
    protected InputHandler input;
    public Animator critAnim;
    public Animator specAnim;

    [Header("Game State Variables")]
    protected int randomAction;

    protected virtual void Start()
    {

    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
    public IEnumerator WaitBossAction()
    {
        if (GameManager.instance.healSkill.isHeal)
        {
            yield return new WaitForSeconds(5f);
            GameManager.instance.enemyTurn = true;
        }
        else
        {
            yield return new WaitForSeconds(3f);
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
    public void ActiveHabPanel()
    {
        GameManager.instance.habilityPanel.SetActive(true);
        if (GameManager.instance.character != null) GameManager.instance.playerTurn = true;
    }
}