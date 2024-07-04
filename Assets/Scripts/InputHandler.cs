using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [Header("Damage Flags")]
    public bool monsterDealDmg;
    public bool playerDealDmg;

    [Header("UI Elements")]
    protected Player player;
    protected Enemy enemy;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UpdateCurrentEnemy();
        monsterDealDmg = false;
        playerDealDmg = false;
    }
    public void UpdateCurrentEnemy()
    {
        enemy = FindCurrentEnemy();
    }
    public Enemy FindCurrentEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var e in enemies)
        {
            if (e.gameObject.activeInHierarchy)
            {
                return e;
            }
        }
        return null;
    }
    public Enemy GetCurrentEnemy()
    {
        return enemy;
    }
    public void ValidateInput(string buttonText)
    {
        GameManager.instance.skills.question = "";

        if (GameManager.instance.habilityPanel != null)
        {
            GameManager.instance.questionPanel.SetActive(false);
            GameManager.instance.isCalc = false;

            GameManager.instance.OnTurnEnd();
        }

        if (!int.TryParse(buttonText, out int _answer))
        {
            Debug.LogError("Failed to parse buttonText to int");
            return;
        }

        if (_answer == GameManager.instance.skills.answer)
        {
            if (GameManager.instance.character != null) GameManager.instance.character.playerTurn = false;

            if (GameManager.instance.basicSkill != null && GameManager.instance.basicSkill.isBasic)
            {
                playerDealDmg = true;
                player.anim.SetTrigger("NormalAttack");
                GameManager.instance.basicSkill.isBasic = false;
                StartCoroutine(nameof(ActiveHabPanel));
            }
            else if (GameManager.instance.healSkill != null && GameManager.instance.healSkill.isHeal)
            {
                playerDealDmg = false;
                GameManager.instance.healSkill.isHeal = false;
                StartCoroutine(nameof(HealEvent));
            }
            else if (GameManager.instance.advancedSkill != null && GameManager.instance.advancedSkill.isAdvanced)
            {
                playerDealDmg = true;
                player.anim.SetTrigger("SpecialAttack");
                StartCoroutine(nameof(PlayerDoubleDmg));
                GameManager.instance.advancedSkill.isAdvanced = false;
                StartCoroutine(nameof(ActiveHabPanel));
            }
        }
        else if (_answer != GameManager.instance.skills.answer)
        {
            if (GameManager.instance.character != null) GameManager.instance.character.playerTurn = false;

            GameManager.instance.advancedSkill.isAdvanced = false;
            GameManager.instance.basicSkill.isBasic = false;
            GameManager.instance.healSkill.isHeal = false;
        }
    }
    public IEnumerator PlayerDoubleDmg()
    {
        player.atqDmg *= 2;
        yield return new WaitForSeconds(2);
        player.atqDmg /= 2;
    }
    public IEnumerator HealEvent()
    {
        player.healAnim.SetTrigger("Heal");
        StartCoroutine(nameof(ActiveHabPanel));
        yield return new WaitForSeconds(1.5f);
        player.Life.GetHeal(player.Life.healValue);
    }
    public IEnumerator ActiveHabPanel()
    {
        yield return new WaitForSeconds(3f);
        GameManager.instance.habilityPanel.SetActive(true);
        if (GameManager.instance.character != null) GameManager.instance.character.playerTurn = true;
    }
}