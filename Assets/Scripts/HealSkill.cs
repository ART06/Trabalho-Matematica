using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skills
{
    public bool isHeal;

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
        question += $"{squaredNumber}²";

        switch (calcTypeMD)
        {
            case CalcTypeMD.Multi:
                preAnswer = specialNumbers1 * specialNumbers2;
                question += $" * {specialNumbers2}";
                break;

            case CalcTypeMD.Division:
                if (specialNumbers2 == 0)
                {
                    Debug.LogError("Divisão por zero evitada");
                    return;
                }

                while (true)
                {
                    if (specialNumbers1 % specialNumbers2 == 0)
                    {
                        preAnswer = specialNumbers1 / specialNumbers2;
                        question += $" : {specialNumbers2}";
                        break;
                    }

                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        Debug.LogError("Loop infinito evitado");
                        return;
                    }

                    specialNumbers1 = Random.Range(1, 100); // Exemplo de ajuste
                }
                break;
        }

        switch (calcTypePM)
        {
            case CalcTypePM.Plus:
                answer = (int)preAnswer + basicNumbers;
                questionText.text = $"{question} + {basicNumbers} = ?";
                break;

            case CalcTypePM.Minus:
                answer = (int)preAnswer - basicNumbers;
                questionText.text = $"{question} - {basicNumbers} = ?";
                break;
        }

        if (questionText != null)
        {
            Debug.Log(questionText.text);
        }
        else
        {
            Debug.LogError("questionText é nulo");
        }
    }
}