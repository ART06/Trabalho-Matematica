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
            squaredNumber = Random.Range(2, 21);
            specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
            specialNumbers2 = Random.Range(2, 21);
            question = $"{squaredNumber}²";

            if (calcMD == 0)
            {
                preAnswer = specialNumbers1 * specialNumbers2;
                question += $" * {specialNumbers2}";
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
                        question = $"{specialNumbers1} : {specialNumbers2}";
                        break;
                    }
                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        calculationSuccessful = false;
                        break;
                    }
                    specialNumbers2 = Random.Range(2, 21);
                }
            }
        }
        while (!calculationSuccessful);
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