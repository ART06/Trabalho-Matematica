using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    [Header("UI Elements")]

    [Header("Damage Flags")]
    public bool monsterDealDmg;
    public bool playerDealDmg;

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
        if (GameManager.instance.habilityPanel != null) GameManager.instance.questionPanel.SetActive(false);
        if (GameManager.instance.habilityPanel != null) GameManager.instance.habilityPanel.SetActive(true);

        int _answer = int.Parse(buttonText);


        if (_answer == GameManager.instance.skills.answer)
        {
            playerDealDmg = true;
            if (GameManager.instance.advancedSkill.isAdvanced)
            {
                player.anim.SetTrigger("SpecialAttack");
                StartCoroutine(nameof(PlayerDoubleDmg));
                GameManager.instance.advancedSkill.isAdvanced = false;
            }
            player.anim.SetTrigger("NormalAttack");
            GameManager.instance.basicSkill.isBasic = false;
        }
        else
        {
            monsterDealDmg = true;
            if (enemy != null) enemy.anim.SetTrigger("Attack");
        }
    }
    public IEnumerator PlayerDoubleDmg()
    {
        player.atqDmg *= 2;
        yield return new WaitForSeconds(2);
        player.atqDmg /= 2;
    }
}