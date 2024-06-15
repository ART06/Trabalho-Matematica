using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedSkill : Skills
{
    public bool isAdvanced;

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        sqrRootNumber = UnityEngine.Random.Range(1, 100);
        squaredNumber = UnityEngine.Random.Range(1, 30);

        switch (calcTypeSS)
        {
            case CalcTypeSS.SqrRoot:
                int maxIterations = 1000;
                int iterationCount = 0;

                while (true)
                {
                    sqrRootNumber = (int)Mathf.Sqrt(sqrRootNumber);
                    if (sqrRootNumber == Mathf.Floor(sqrRootNumber))
                    {
                        specialNumbers1 = sqrRootNumber;
                        question += $"√{sqrRootNumber}";
                        break;
                    }

                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        Debug.LogError("Loop infinito evitado");
                        return;
                    }

                    sqrRootNumber = UnityEngine.Random.Range(1, 100); // Exemplo de ajuste
                }
                break;

            case CalcTypeSS.Squared:
                specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                question += $"{squaredNumber}²";
                break;
        }

        switch (calcTypeMD)
        {
            case CalcTypeMD.Multi:
                question += $" *";
                break;

            case CalcTypeMD.Division:
                question += $" :";
                break;
        }

        switch (calcTypeSS)
        {
            case CalcTypeSS.SqrRoot:

                while (true)
                {
                    sqrRootNumber = (int)Mathf.Sqrt(sqrRootNumber);
                    if (sqrRootNumber == Mathf.Floor(specialNumbers2))
                    {
                        specialNumbers2 = sqrRootNumber;
                        question += $" √{sqrRootNumber}";
                        break;
                    }

                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        Debug.LogError("Loop infinito evitado");
                        return;
                    }

                    sqrRootNumber = UnityEngine.Random.Range(1, 100); // Exemplo de ajuste
                }
                break;

            case CalcTypeSS.Squared:
                specialNumbers2 = (int)Mathf.Pow(squaredNumber, 2);
                question += $" {squaredNumber}²";
                break;
        }

        if (calcTypeMD == CalcTypeMD.Multi)
        {
            preAnswer = specialNumbers1 * specialNumbers2;
            question += $" {specialNumbers2}";
        }
        else if (calcTypeMD == CalcTypeMD.Division)
        {
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
                    question += $" {specialNumbers2}";
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