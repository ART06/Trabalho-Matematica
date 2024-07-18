using UnityEngine;
using UnityEngine.UI;

public class BasicSkill : Skills
{
    public bool isBasic;
    public Button basic;

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        bool calculationSuccessful;
        isBasic = true;
        do
        {
            calculationSuccessful = true;
            iterationCount = 0;
            specialNumbers1 = Random.Range(5, 61);
            specialNumbers2 = Random.Range(2, 21);

            if (calcMD == 0)
            {
                preAnswer = specialNumbers1 * specialNumbers2;
                question = $"{specialNumbers1} * {specialNumbers2}";
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
}