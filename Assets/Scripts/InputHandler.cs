using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    public TextMeshProUGUI resultText;

    public void ValidateInput()
    {
        string input = inputField.text;

        if (float.TryParse(input, out float playerAnswer))
        {
            bool isCorrect = GameManager.instance.CheckAnswer(playerAnswer);
            if (isCorrect)
            {
                resultText.text = "Resposta certa!";
                resultText.color = Color.green;
            }
            else
            {
                resultText.text = "Burro pra caralho kkkkkk!";
                resultText.color = Color.red;
            }
        }
        else
        {
            resultText.text = "Escreve direito porra";
            resultText.color = Color.gray;
        }
    }
}