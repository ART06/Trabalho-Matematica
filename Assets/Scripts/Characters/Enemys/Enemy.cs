using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [Header("UI Elements")]
    [HideInInspector] public FirstBoss firstEnemy;
    [HideInInspector] public SecondBoss secondEnemy;
    [HideInInspector] public ThirdBoss thirdEnemy;
    public int roundCooldown;
    public int remainCooldown;
    protected InputHandler input;
    public Animator critAnim;
    public Animator specAnim;
    public TextMeshProUGUI cooldownText;
    public GameObject cooldownPanel;
    public GameObject enemySkill;

    [Header("Game State Variables")]
    public bool specIsOnCooldown;
    protected int randomAction;

    protected virtual void Start()
    {
        firstEnemy = GetComponent<FirstBoss>();
        secondEnemy = GetComponent<SecondBoss>();
        thirdEnemy = GetComponent<ThirdBoss>();

        input = GetComponent<InputHandler>();
        if (cooldownPanel != null)
            cooldownPanel.SetActive(false);
        if (GameManager.instance.isFighting && cooldownText != null)
            cooldownText.text = remainCooldown.ToString();
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
        Debug.Log(randomAction);
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
    public void UpdateCooldown()
    {
        if (remainCooldown > 0)
        {
            remainCooldown -= 1;
            if (remainCooldown <= 0)
            {
                specIsOnCooldown = false;
                remainCooldown = 0;
            }
        }
    }
}