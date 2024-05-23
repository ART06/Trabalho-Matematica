using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public enum CalcType { Plus, Minus, Multi, Division };
    public CalcType calcType;

    public static GameManager instance;
    protected Player player;
    protected Enemy enemy;
    protected InputHandler inputhandler;

    public bool isFighting;
    private bool questionGenerated = false;

    public string operators = "+-*/";
    public string operatorSymbol;
    int firstNumber;
    int secondNumber;
    public float answer;
    public string formatedAnswer;

    public GameObject calcPanel;
    public GameObject lifePanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI asnwerVerifyText;

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
        lifePanel.SetActive(false);
        asnwerVerifyText.gameObject.SetActive(false);
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        inputhandler = GetComponent<InputHandler>();

        if (player == null)
        {
            Debug.LogError("Player n�o encontrado na cena");
        }
        if (enemy == null)
        {
            Debug.LogError("Enemy n�o encontrado na cena");
        }
    }

    private void FixedUpdate()
    {
        CombatManager();
        inputhandler.ValidateInput();
    }
    protected void CombatManager()
    {
        if (enemy != null && enemy.isFightingPlayer)
        {
            calcPanel.SetActive(true);
            lifePanel.SetActive(true);

            if (!questionGenerated)
            {
                calcType = RandomOperator();
                RandomCalculator();
                questionGenerated = true;
                Debug.Log(answer);
            }
            //player.canMove = false;
        }
        else if (calcPanel != null)
        {
            inputhandler.inputField.text = "";
            asnwerVerifyText.gameObject.SetActive(false);
            calcPanel.SetActive(false);
            lifePanel.SetActive(false);
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
                answer = Mathf.Round(firstNumber / secondNumber);
                questionText.text = firstNumber + " : " + secondNumber + " = ?";
                break;
        }
    }
    CalcType RandomOperator()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcType));
        System.Random _random = new (); //() = System.Random() (serve s� para evitar redund�ncia)
        CalcType _randomOperator = (CalcType)_operators.GetValue(_random.Next(_operators.Length));
        return _randomOperator;
    }
    public bool CheckAnswer(float playerAnswer)
    {
        return Mathf.Approximately(answer, playerAnswer);
    }
}