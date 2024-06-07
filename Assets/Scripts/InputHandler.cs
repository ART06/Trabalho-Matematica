using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField inputField;
    public TextMeshProUGUI resultText;

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

    public void ValidateInput()
    {
        string input = inputField.text;

        if (float.TryParse(input, out float playerAnswer) && (GameManager.instance.asnwerVerifyText.IsActive() || GameManager.instance.remainTime < 0))
        {
            GameManager.instance.remainTime = GameManager.instance.maxTime;
            GameManager.instance.questionPanel.SetActive(false);
            Invoke(nameof(DeactivateText), 2.0f);
            bool isCorrect = GameManager.instance.CheckAnswer(playerAnswer);
            if (isCorrect && !playerDealDmg)
            {
                playerDealDmg = true;
                if (GameManager.instance.remainTime > GameManager.instance.maxTime / 2)
                {
                    player.anim.SetTrigger("SpecialAttack");
                    StartCoroutine(nameof(PlayerDoubleDmg));
                }
                player.anim.SetTrigger("NormalAttack");
                resultText.text = "Resposta certa!";
                resultText.color = Color.green;

                GameManager.instance.questionGenerated = false;
                GameManager.instance.RegenerateAnswer();
            }
            else if (!isCorrect && !monsterDealDmg || GameManager.instance.remainTime < 0 && !monsterDealDmg)
            {
                monsterDealDmg = true;
                if (enemy != null) enemy.anim.SetTrigger("Attack");
                resultText.color = Color.red;
                GameManager.instance.questionGenerated = false;
                GameManager.instance.RegenerateAnswer();

                if (GameManager.instance.remainTime < 0)
                {
                    StartCoroutine(nameof(EnemyDoubleDmg));
                    resultText.text = "Acabou o tempo!";
                }
                else if (!isCorrect) resultText.text = "Resposta errada!";
            }
        }
        else if (!playerDealDmg && !monsterDealDmg)
        {
            playerDealDmg = false;
            monsterDealDmg = false;
            resultText.text = "Resposta inválida, tente novamente.";
            resultText.color = Color.gray;
        }
    }

    private void DeactivateText()
    {
        resultText.gameObject.SetActive(false);
    }

    public IEnumerator EnemyDoubleDmg()
    {
        enemy.atqDmg *= 2;
        yield return new WaitForSeconds(2);
        enemy.atqDmg /= 2;
    }

    public IEnumerator PlayerDoubleDmg()
    {
        player.atqDmg *= 2;
        yield return new WaitForSeconds(2);
        player.atqDmg /= 2;
    }
}
