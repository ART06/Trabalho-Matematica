using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public enum CalcType {Plus, Minus, Multi, Division};
    public CalcType calcType;

    public static GameManager instance;
    protected Player player;
    protected Enemy enemy;

    public bool isFighting;

    public string operators = "+-*/";
    public string operatorSymbol;
     int firstNumber;
    int secondNumber;
    public float answer;

    public GameObject calcPanel;

    public void Awake()
    {
        if (instance == null)
        {
            instance = null;
        }
        else Destroy(gameObject);
    }
    protected void Start()
    {
        calcPanel.SetActive(false);
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();
    }
    protected void Update()
    {
        CombatManager();
    }
    protected void CombatManager()
    {
        if (enemy.isFightingPlayer)
        {
            calcPanel.SetActive(true);
            //CombatManager();
            CalcType _randomOperator = RandomOperator();
            player.canMove = false;
        }
        else
        {
            calcPanel.SetActive(false);
            enemy.isFightingPlayer = false;
        }
    }
    protected void RandomCalculator()
    {
        operatorSymbol += operators[Random.Range(0, operators.Length)];
        Debug.Log(operatorSymbol);

        switch (calcType)
        {
            case CalcType.Plus:
                firstNumber += Random.Range(1, 100);
                secondNumber += Random.Range(1, 10);
                answer = firstNumber + secondNumber;
                break;

            case CalcType.Minus:
                firstNumber += Random.Range(1, 100);
                secondNumber += Random.Range(1, 10);
                answer = firstNumber - secondNumber;
                break;

            case CalcType.Multi:
                firstNumber += Random.Range(1, 100);
                secondNumber += Random.Range(1, 10);
                answer = firstNumber * secondNumber;
                break;

            case CalcType.Division:
                firstNumber += Random.Range(1, 100);
                secondNumber += Random.Range(1, 10);
                answer = firstNumber + secondNumber;
                break;
        }
    }
    CalcType RandomOperator()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcType));
        System.Random _random = new System.Random();
        CalcType _randomOperator = (CalcType)_operators.GetValue(_random.Next(_operators.Length));
        return _randomOperator;
    }
}