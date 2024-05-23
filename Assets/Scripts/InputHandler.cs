using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    public TextMeshProUGUI resultText;
    public bool monsterDealDmg;
    public bool playerDealDmg;

    protected Player player;
    protected Enemy enemy;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        monsterDealDmg = false;
        playerDealDmg = false;
    }
    public void ValidateInput()
    {
        string input = inputField.text;

        if (float.TryParse(input, out float playerAnswer) && GameManager.instance.asnwerVerifyText.IsActive())
        {
            GameManager.instance.questionPanel.SetActive(false);
            Invoke(nameof(DeactivateText), 2.0f);
            bool isCorrect = GameManager.instance.CheckAnswer(playerAnswer);
            if (isCorrect && !playerDealDmg)
            {
                playerDealDmg = true;
                player.anim.SetTrigger("NormalAttack");
                resultText.text = "Resposta certa!";
                resultText.color = Color.green;
                GameManager.instance.questionGenerated = false;
                GameManager.instance.RegenerateAnswer();
            }
            else if (!isCorrect && !monsterDealDmg)
            {
                monsterDealDmg = true;
                enemy.anim.SetTrigger("Attack");
                resultText.text = "Resposta errada!";
                resultText.color = Color.red;
                GameManager.instance.questionGenerated = false;
                GameManager.instance.RegenerateAnswer();
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
}