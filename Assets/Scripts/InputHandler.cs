using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    public TextMeshProUGUI resultText;
    public bool monsterDealDmg;
    public bool playerDealDmg;

    public void ValidateInput()
    {
        string input = inputField.text;

        if (float.TryParse(input, out float playerAnswer))
        {
            bool isCorrect = GameManager.instance.CheckAnswer(playerAnswer);
            if (isCorrect)
            {
                playerDealDmg = true;
                monsterDealDmg = false;
                resultText.text = "Resposta certa!";
                resultText.color = Color.green;
            }
            else
            {
                monsterDealDmg = true;
                playerDealDmg = false;
                resultText.text = "Resposta errada!";
                resultText.color = Color.red;
            }
        }
        else
        {
            playerDealDmg = false;
            monsterDealDmg = false;
            resultText.text = "Resposta inválida, tente novamente.";
            resultText.color = Color.gray;
        }
    }
}