using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [Header("UI Elements")]
    public int roundCooldown;
    public int remainCooldown;
    protected InputHandler input;
    public Animator critAnim;
    public Animator specAnim;
    public TextMeshProUGUI cooldownText;
    public GameObject cooldownPanel;

    [Header("Game State Variables")]
    [HideInInspector] public bool specIsOnCooldown;
    protected int randomAction;

    protected virtual void Start()
    {
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
            yield return new WaitForSeconds(4f);
            GameManager.instance.enemyTurn = true;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            GameManager.instance.enemyTurn = true;
        }
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
    public void OnTurnEnd()
    {
        if (specIsOnCooldown)
        {
            remainCooldown--;
            if (cooldownText != null) cooldownText.text = remainCooldown.ToString();
            if (remainCooldown <= 0)
            {
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