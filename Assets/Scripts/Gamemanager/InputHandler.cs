using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("UI Elements")]
    protected Player player;
    protected Enemy enemy;
    public bool wrongAnswer;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UpdateCurrentEnemy();
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
        GameManager.instance.skills.answer.ToString("");

        if (GameManager.instance.habilityPanel != null)
        {
            GameManager.instance.questionPanel.SetActive(false);
            GameManager.instance.isCalc = false;

            GameManager.instance.OnTurnEnd();
        }
        if (!int.TryParse(buttonText, out int _answer)) return;

        if (_answer == GameManager.instance.correctAnswer)
        {
            if (GameManager.instance.character != null) GameManager.instance.playerTurn = false;
            StartCoroutine(enemy.WaitBossAction());

            if (GameManager.instance.basicSkill != null && GameManager.instance.basicSkill.isBasic)
            {
                player.anim.SetTrigger("NormalAttack");
                GameManager.instance.basicSkill.isBasic = false;
            }
            else if (GameManager.instance.healSkill != null && GameManager.instance.healSkill.isHeal)
            {
                GameManager.instance.healSkill.isHeal = false;
                StartCoroutine(nameof(HealEvent));
            }
            else if (GameManager.instance.advancedSkill != null && GameManager.instance.advancedSkill.isAdvanced)
            {
                player.anim.SetTrigger("SpecialAttack");
                StartCoroutine(nameof(PlayerDoubleDmg));
                GameManager.instance.advancedSkill.isAdvanced = false;
            }
        }
        else if (_answer != GameManager.instance.skills.answer || wrongAnswer)
        {
            if (GameManager.instance.character != null) GameManager.instance.playerTurn = false;
            StartCoroutine(enemy.WaitBossAction());

            GameManager.instance.advancedSkill.isAdvanced = false;
            GameManager.instance.basicSkill.isBasic = false;
            GameManager.instance.healSkill.isHeal = false;
            wrongAnswer = false;
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
        yield return new WaitForSeconds(1.5f);
        player.Life.GetHeal(player.Life.healValue);
    }
}