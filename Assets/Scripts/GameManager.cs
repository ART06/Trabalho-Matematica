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
    protected Character character;
    protected FirstBoss firstBoss;
    protected SecondBoss secondBoss;
    protected ThirdBoss thirdBoss;

    public bool isFighting;
    public bool firstEncounter;
    [HideInInspector] public bool questionGenerated = false;

    public string operators = "+-*/";
    public string operatorSymbol;
    int firstNumber;
    int secondNumber;
    public float answer;
    public string formatedAnswer;

    public GameObject calcPanel;
    public GameObject lifePanel;
    public GameObject questionPanel;
    public GameObject deathPanel;
    public GameObject fb;
    public GameObject sb;
    public GameObject tb;
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
        firstEncounter = false;
        calcPanel.SetActive(false);
        lifePanel.SetActive(false);
        deathPanel.SetActive(false);
        sb.SetActive(false);
        tb.SetActive(false);
        asnwerVerifyText.gameObject.SetActive(false);
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        inputhandler = GetComponent<InputHandler>();
        firstBoss = GetComponent<FirstBoss>();
        secondBoss = GetComponent<SecondBoss>();
        thirdBoss = GetComponent<ThirdBoss>();

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
        ActiveObjects();
        inputhandler.ValidateInput();
    }
    protected void CombatManager()
    {
        if (enemy.isFightingPlayer)
        {
            calcPanel.SetActive(true);
            lifePanel.SetActive(true);

            if (!questionGenerated && !firstEncounter)
            {
                calcType = RandomOperator();
                RandomCalculator();
                questionGenerated = true;
                Debug.Log(answer);
                firstEncounter = true;
            }
            player.canMove = false;
        }
        else if (calcPanel != null)
        {
            inputhandler.inputField.text = "";
            asnwerVerifyText.gameObject.SetActive(false);
            calcPanel.SetActive(false);
            lifePanel.SetActive(false);
            enemy.isFightingPlayer = false;
            questionGenerated = false;
            player.canMove = true;
        }
    }
    public void RegenerateAnswer()
    {
        if (player.IsDead() == true) return;
        Invoke(nameof(ActivatePanel), 2f);
        calcType = RandomOperator();
        RandomCalculator();
        questionGenerated = true;
        inputhandler.inputField.text = "";
        Debug.Log(answer);
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
        System.Random _random = new (); //() = System.Random() (serve só para evitar redundância)
        CalcType _randomOperator = (CalcType)_operators.GetValue(_random.Next(_operators.Length));
        return _randomOperator;
    }
    public bool CheckAnswer(float playerAnswer)
    {
        return Mathf.Approximately(answer, playerAnswer);
    }
    private void ActivatePanel()
    {
        questionPanel.SetActive(true);
        inputhandler.playerDealDmg = false;
        inputhandler.monsterDealDmg = false;
    }
    protected void ActiveObjects()
    {
        if (firstBoss != null && firstBoss.firstBossClear == true) sb.SetActive(true);
        if (secondBoss != null && secondBoss.secondBossClear == true) tb.SetActive(true);
    }
}