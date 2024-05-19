using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditor;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public enum CalcType { Plus, Minus, Multi, Division };
    public CalcType calcType;

    public static GameManager instance;
    protected Player player;
    protected Enemy enemy;

    public bool isFighting;
    private bool questionGenerated = false;

    public string operators = "+-*/";
    public string operatorSymbol;
    int firstNumber;
    int secondNumber;
    public float answer;

    public GameObject calcPanel;
    [SerializeField] TextMeshProUGUI questionText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    protected void Start()
    {
        calcPanel.SetActive(false);
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();

        if (player == null)
        {
            Debug.LogError("Player não encontrado na cena");
        }
        if (enemy == null)
        {
            Debug.LogError("Enemy não encontrado na cena");
        }
    }

    private void FixedUpdate()
    {
        CombatManager();
    }
    protected void CombatManager()
    {
        if (enemy != null && enemy.isFightingPlayer)
        {
            calcPanel.SetActive(true);

            if (!questionGenerated)
            {
                calcType = RandomOperator();
                RandomCalculator();
                questionGenerated = true;
            }
            //player.canMove = false;
        }
        else if (calcPanel != null)
        {
            calcPanel.SetActive(false);
            enemy.isFightingPlayer = false;
            questionGenerated = false;
        }
    }
    protected void RandomCalculator()
    {
        operatorSymbol = operators[Random.Range(0, operators.Length)].ToString();

        firstNumber = Random.Range(1, 100);
        secondNumber = Random.Range(1, 10);
        switch (calcType)
        {
            case CalcType.Plus:
                answer = firstNumber + secondNumber;
                questionText.text = firstNumber + " + " + secondNumber + " = ?";
                break;

            case CalcType.Minus:
                answer = firstNumber - secondNumber;
                questionText.text = firstNumber + " - " + secondNumber + " = ?";
                break;

            case CalcType.Multi:
                answer = firstNumber * secondNumber;
                questionText.text = firstNumber + " * " + secondNumber + " = ?";
                break;

            case CalcType.Division:
                answer = firstNumber / (float)secondNumber;
                questionText.text = firstNumber + " : " + secondNumber + " = ?";
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