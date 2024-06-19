using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
            specialNumbers1 = Random.Range(5, 60);
            specialNumbers2 = Random.Range(2, 20);
            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    preAnswer = specialNumbers1 * specialNumbers2;
                    question = $"{specialNumbers1} * {specialNumbers2}";
                    break;
                case CalcTypeMD.Division:
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
}