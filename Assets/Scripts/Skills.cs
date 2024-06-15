using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Skills : MonoBehaviour
{
    // Enums
    public ActionType actionType;
    public enum ActionType { basic, heal, advanced }

    public CalcTypeMD calcTypeMD;
    public CalcTypePM calcTypePM;
    public CalcTypeSS calcTypeSS;
    public enum CalcTypeMD { Multi, Division }
    public enum CalcTypePM { Plus, Minus }
    public enum CalcTypeSS { SqrRoot, Squared }

    [Header("Calculation Variables")]
    protected int specialNumbers1;
    protected int specialNumbers2;
    protected int basicNumbers;
    protected int squaredNumber;
    protected int sqrRootNumber;
    [HideInInspector] public int answerPlace;
    public int answer;
    protected float preAnswer;
    [HideInInspector] public string question = "";
    [HideInInspector] public int maxIterations;
    [HideInInspector] public int iterationCount;

    [Header("UI Elements")]
    public TextMeshProUGUI questionText;

    public void GenerateAnswer()
    {
        calcTypePM = RandomOperator1();
        calcTypeMD = RandomOperator2();
        calcTypeSS = RandomOperator3();

        answerPlace = Random.Range(0, 2);

        question = "";
    }
    public virtual void ActivateSkill()
    {
        if (GameManager.instance.questionPanel != null) GameManager.instance.questionPanel.SetActive(true);
        if (GameManager.instance.habilityPanel != null) GameManager.instance.habilityPanel.SetActive(false);
        GameManager.instance.ActivatePanel();
        GenerateAnswer();

        specialNumbers1 = UnityEngine.Random.Range(5, 60);
        specialNumbers2 = UnityEngine.Random.Range(5, 60);
        basicNumbers = UnityEngine.Random.Range(5, 100);

        maxIterations = 1000;
        iterationCount = 0;
    }
    public CalcTypePM RandomOperator1()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypePM));
        System.Random _random = new();
        return (CalcTypePM)_operators.GetValue(_random.Next(_operators.Length));
    }
    public CalcTypeMD RandomOperator2()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypeMD));
        System.Random _random = new();
        return (CalcTypeMD)_operators.GetValue(_random.Next(_operators.Length));
    }
    public CalcTypeSS RandomOperator3()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypeSS));
        System.Random _random = new();
        return (CalcTypeSS)_operators.GetValue(_random.Next(_operators.Length));
    }
}