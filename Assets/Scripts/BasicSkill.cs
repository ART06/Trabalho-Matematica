using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BasicSkill : Skills
{
    public bool isBasic;

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        switch (calcTypeMD)
        {
            case CalcTypeMD.Multi:
                preAnswer = specialNumbers1 * specialNumbers2;
                question += $"{specialNumbers1} * {specialNumbers2}";
                break;

            case CalcTypeMD.Division:
                // Certifique-se de que specialNumbers2 n�o seja zero para evitar divis�o por zero
                if (specialNumbers2 == 0)
                {
                    Debug.LogError("Divis�o por zero evitada");
                    return;
                }

                // Limite o n�mero de itera��es do loop para evitar loops infinitos

                while (true)
                {
                    if (specialNumbers1 % specialNumbers2 == 0)
                    {
                        preAnswer = specialNumbers1 / specialNumbers2;
                        question += $"{specialNumbers1} : {specialNumbers2}";
                        break;
                    }

                    // Verifique se o n�mero m�ximo de itera��es foi atingido
                    iterationCount++;
                    if (iterationCount >= maxIterations)
                    {
                        Debug.LogError("Loop infinito evitado");
                        return;
                    }

                    // Opcional: Modificar specialNumbers1 ou specialNumbers2 para tentar evitar loop infinito
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

        // Verifique se questionText n�o � nulo antes de us�-lo
        if (questionText != null)
        {
            Debug.Log(questionText.text);
        }
        else
        {
            Debug.LogError("questionText � nulo");
        }
    }
}