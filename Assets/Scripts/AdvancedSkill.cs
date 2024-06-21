using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedSkill : Skills
{
    public bool isAdvanced;
    public Button advanced;
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
            if (advanced != null) advanced.gameObject.SetActive(false);
        }

        bool calculationSuccessful;
        isAdvanced = true;
        do
        {
            calculationSuccessful = true;
            iterationCount = 0;
            sqrRootNumber = Random.Range(4, 100);
            squaredNumber = Random.Range(2, 20);
            int root;

            switch (calcTypeSS)
            {
                case CalcTypeSS.SqrRoot:
                    while (true)
                    {
                        root = (int)Mathf.Sqrt(sqrRootNumber);
                        if (root * root == sqrRootNumber)
                        {
                            specialNumbers1 = root;
                            question = $"√{sqrRootNumber}";
                            break;
                        }
                        iterationCount++;
                        if (iterationCount >= maxIterations)
                        {
                            Debug.Log("Loop infinito evitado, tentando novamente...");
                            calculationSuccessful = false;
                            break;
                        }
                        sqrRootNumber = Random.Range(4, 100);
                    }
                    break;
                case CalcTypeSS.Squared:
                    specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                    question = $"{squaredNumber}²";
                    break;
            }

            if (!calculationSuccessful) continue;

            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    question += " * ";
                    break;

                case CalcTypeMD.Division:
                    question += " : ";
                    break;
            }

            calcTypeSS = RandomOperator3();
            sqrRootNumber = Random.Range(4, 100);
            squaredNumber = Random.Range(2, 20);

            switch (calcTypeSS)
            {
                case CalcTypeSS.SqrRoot:
                    while (true)
                    {
                        root = (int)Mathf.Sqrt(sqrRootNumber);
                        if (root * root == sqrRootNumber)
                        {
                            specialNumbers2 = root;
                            question += $"√{sqrRootNumber}";
                            break;
                        }

                        iterationCount++;
                        if (iterationCount >= maxIterations)
                        {
                            Debug.Log("Loop infinito evitado, tentando novamente...");
                            calculationSuccessful = false;
                            break;
                        }
                        sqrRootNumber = Random.Range(4, 100);
                    }
                    break;
                case CalcTypeSS.Squared:
                    specialNumbers2 = (int)Mathf.Pow(squaredNumber, 2);
                    question += $"{squaredNumber}²";
                    break;
            }

            if (!calculationSuccessful) continue;

            if (calcTypeMD == CalcTypeMD.Multi)
            {
                preAnswer = specialNumbers1 * specialNumbers2;
            }
            else if (calcTypeMD == CalcTypeMD.Division)
            {
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
                        break;
                    }
                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        Debug.Log("Loop infinito evitado, tentando novamente...");
                        calculationSuccessful = false;
                        break;
                    }
                    squaredNumber = Random.Range(2, 20);
                    specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                }
            }
        } while (!calculationSuccessful);

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
            if (cooldownText != null)
                cooldownText.text = remainCooldown.ToString();

            if (remainCooldown <= 0)
            {
                if (advanced != null)
                    advanced.gameObject.SetActive(true);
                if (cooldownPanel != null)
                    cooldownPanel.SetActive(false);
                isOnCooldown = false;
            }
        }
    }
}