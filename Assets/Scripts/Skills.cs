using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

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
    public float answer;
    protected float preAnswer;
    [HideInInspector] public string question = "";
    [HideInInspector] public int maxIterations;
    [HideInInspector] public int iterationCount;
    [HideInInspector] public int rightPos;
    [HideInInspector] public int offset;

    [Header("UI Elements")]
    [SerializeField] internal TextMeshProUGUI questionText;
    public int roundCooldown;  // Corrigido o nome para roundCooldown
    public int remainCooldown; // Corrigido o nome para remainCooldown

    [Header("Game State Variables")]
    [HideInInspector] public bool isOnCooldown;
    public bool canCooldown;

    public virtual void Start()
    {
        isOnCooldown = false;
    }

    public void GenerateAnswer()
    {
        calcTypePM = RandomOperator1();
        calcTypeMD = RandomOperator2();
        calcTypeSS = RandomOperator3();
        GameManager.instance.RightAnswerPos();

        rightPos = Random.Range(0, 3);
    }

    public virtual void ActivateSkill()
    {
        if (isOnCooldown)
        {
            Debug.Log("Skill is on cooldown.");
            return;
        }

        if (GameManager.instance.questionPanel != null) GameManager.instance.questionPanel.SetActive(true);
        GameManager.instance.isCalc = true;
        if (GameManager.instance.habilityPanel != null) GameManager.instance.habilityPanel.SetActive(false);
        GameManager.instance.ActivatePanel();

        specialNumbers1 = Random.Range(5, 60);
        specialNumbers2 = Random.Range(5, 60);
        basicNumbers = Random.Range(5, 100);

        maxIterations = 1000;
        iterationCount = 0;
        remainCooldown = roundCooldown;

        GenerateAnswer();
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

    public void UpdateCooldown()
    {
        if (remainCooldown > 0)
        {
            remainCooldown -= 1;
            if (remainCooldown <= 0)
            {
                isOnCooldown = false;
                remainCooldown = 0;
            }
        }
    }
}