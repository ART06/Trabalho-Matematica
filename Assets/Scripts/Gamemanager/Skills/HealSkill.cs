using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealSkill : Skills
{
    public bool isHeal;
    public Button heal;
    public TextMeshProUGUI cooldownText;
    public GameObject cooldownPanel;
    public override void Start()
    {
        base.Start();
        if (cooldownPanel != null)
            cooldownPanel.SetActive(false);
        if (GameManager.instance.isFighting && cooldownText != null)
            cooldownText.text = remainCooldown.ToString();
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        if (canCooldown && !isOnCooldown)
        {
            remainCooldown = roundCooldown;
            isOnCooldown = true;
            if (cooldownPanel != null) cooldownPanel.SetActive(true);
            if (heal != null) heal.gameObject.SetActive(false);
        }

        bool calculationSuccessful;
        isHeal = true;
        do
        {
            calculationSuccessful = true;
            iterationCount = 0;
            squaredNumber = Random.Range(2, 20);
            specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
            question = $"{squaredNumber}²";
            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    specialNumbers2 = Random.Range(2, 20);
                    preAnswer = specialNumbers1 * specialNumbers2;
                    question += $" * {specialNumbers2}";
                    break;
                case CalcTypeMD.Division:
                    specialNumbers2 = Random.Range(2, 20);
                    while (true)
                    {
                        if (specialNumbers2 == 0)
                        {
                            Debug.Log("Divisão por zero evitada, tentando novamente...");
                            calculationSuccessful = false;
                            break;
                        }
                        if (specialNumbers1 % specialNumbers2 != 0)
                        {
                            Debug.Log("Divisão inexata, tentando novamente...");
                            calculationSuccessful = false;
                            break;
                        }
                        if (specialNumbers1 % specialNumbers2 == 0)
                        {
                            preAnswer = specialNumbers1 / specialNumbers2;
                            question = $"{specialNumbers1} : {specialNumbers2}";
                            break;
                        }
                        iterationCount++;
                        if (iterationCount >= maxIterations)
                        {
                            Debug.Log("Loop infinito evitado, tentando novamente...");
                            calculationSuccessful = false;
                            break;
                        }
                        specialNumbers2 = Random.Range(2, 20);
                    }
                    break;
            }
        }
        while (!calculationSuccessful);

        switch (calcTypePM)
        {
            case CalcTypePM.Plus:
                answer = preAnswer + basicNumbers;
                questionText.text = $"{question} + {basicNumbers} = ?";
                break;
            case CalcTypePM.Minus:
                answer = preAnswer - basicNumbers;
                questionText.text = $"{question} - {basicNumbers} = ?";
                break;
        }
    }

    public void OnTurnEnd()
    {
        if (isOnCooldown)
        {
            remainCooldown--;
            if (cooldownText != null) cooldownText.text = remainCooldown.ToString();
            if (remainCooldown <= 0)
            {
                if (heal != null)
                    heal.gameObject.SetActive(true);
                if (cooldownPanel != null)
                    cooldownPanel.SetActive(false);
                isOnCooldown = false;
            }
        }
        else
        {
            isOnCooldown = false;
            remainCooldown = 0;
        }
    }
}