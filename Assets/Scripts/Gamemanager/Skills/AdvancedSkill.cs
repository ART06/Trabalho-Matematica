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
            sqrRootNumber = Random.Range(4, 101);
            squaredNumber = Random.Range(2, 21);
            int root;

            if (calcSS == 0)
            {
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
                        calculationSuccessful = false;
                        break;
                    }
                    sqrRootNumber = Random.Range(4, 101);
                }
            }
            else
            {
                specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                question = $"{squaredNumber}²";
            }

            if (!calculationSuccessful) continue;

            if (calcMD == 0)
            {
                question += " * ";
            }
            else
            {
                question += " : ";
            }

            RandomOperatorSS();
            sqrRootNumber = Random.Range(4, 101);
            squaredNumber = Random.Range(2, 21);

            if (calcSS == 0)
            {
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
                        calculationSuccessful = false;
                        break;
                    }
                    sqrRootNumber = Random.Range(4, 101);
                }
            }
            else
            {
                specialNumbers2 = (int)Mathf.Pow(squaredNumber, 2);
                question += $"{squaredNumber}²";
            }

            if (!calculationSuccessful) continue;

            if (calcMD == 0)
            {
                preAnswer = specialNumbers1 * specialNumbers2;
            }
            else
            {
                while (true)
                {
                    if (specialNumbers2 == 0)
                    {
                        calculationSuccessful = false;
                        break;
                    }
                    if (specialNumbers1 % specialNumbers2 != 0)
                    {
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
                        calculationSuccessful = false;
                        break;
                    }
                    squaredNumber = Random.Range(2, 21);
                    specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                }
            }
        }
        while (!calculationSuccessful) ;
        if (calcPM == 0)
        {
            answer = preAnswer + basicNumbers;
            questionText.text = $"{question} + {basicNumbers} = ?";
        }
        else
        {
            answer = preAnswer - basicNumbers;
            questionText.text = $"{question} - {basicNumbers} = ?";
        }
        FinalAnswer();
    }

    public void OnTurnEnd()
    {
        if (isOnCooldown)
        {
            remainCooldown--;
            if (cooldownText != null) cooldownText.text = remainCooldown.ToString();
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
