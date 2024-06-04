using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Enums
    [HideInInspector]
    public enum CalcType { Plus, Minus, Multi, Division }
    public CalcType calcType;

    // Singleton instance
    public static GameManager instance;

    [Header("Player and Enemy References")]
    protected Player player;
    protected Enemy enemy;
    protected InputHandler inputhandler;
    protected Character character;

    [Header("Boss References")]
    [HideInInspector] public FirstBoss fb;
    [HideInInspector] public SecondBoss sb;
    [HideInInspector] public ThirdBoss tb;

    [Header("Game State Variables")]
    public bool isFighting;
    public bool firstEncounter;
    [HideInInspector] public bool questionGenerated = false;

    [Header("Calculation Variables")]
    public string operators = "+-*/";
    protected string operatorSymbol;
    protected string formatedAnswer;
    protected int firstNumber;
    protected int secondNumber;
    public float remainTime;
    public float maxTime;
    public float answer;

    [Header("UI Elements")]
    public Image timerBar;
    public GameObject calcPanel;
    public GameObject lifePanel;
    public GameObject questionPanel;
    public GameObject deathPanel;
    public GameObject firstBoss;
    public GameObject secondBoss;
    public GameObject thirdBoss;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI asnwerVerifyText;

    protected bool isTransitioning;

    #region Unity Methods
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
        remainTime = maxTime;
        firstEncounter = false;

        InitializeUIElements();

        player = FindObjectOfType<Player>();
        inputhandler = GetComponent<InputHandler>();
        fb = FindObjectOfType<FirstBoss>();
        sb = FindObjectOfType<SecondBoss>();
        tb = FindObjectOfType<ThirdBoss>();

        inputhandler.UpdateCurrentEnemy();
    }

    private void InitializeUIElements()
    {
        if (calcPanel != null) calcPanel.SetActive(false);
        if (lifePanel != null) lifePanel.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
        if (secondBoss != null) secondBoss.SetActive(false);
        if (thirdBoss != null) thirdBoss.SetActive(false);
        if (asnwerVerifyText != null) asnwerVerifyText.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (isFighting)
        {
            CombatManager();
            if (questionGenerated) CalcTimer();
        }
        if (inputhandler != null) inputhandler.ValidateInput();
        Debug.Log($"Está em luta?: {isFighting}");
    }
    #endregion

    #region Combat Management
    protected void CombatManager()
    {
        if (isFighting)
        {
            ActivateCombatUI();

            if (!questionGenerated && !firstEncounter)
            {
                GenerateQuestion();
                firstEncounter = true;
            }
            Debug.Log($"Resposta gerada: {answer}");
            player.canMove = false;
        }
        else
        {
            ResetCombatState();
        }
        if (player.IsDead() == true) calcPanel.SetActive(false);

    }

    private void ActivateCombatUI()
    {
        if (calcPanel != null) calcPanel.SetActive(true);
        if (lifePanel != null) lifePanel.SetActive(true);
    }

    private void GenerateQuestion()
    {
        calcType = RandomOperator();
        RandomCalculator();
        questionGenerated = true;
    }

    private void ResetCombatState()
    {
        remainTime = maxTime;
        isTransitioning = true;
        inputhandler.inputField.text = "";
        questionGenerated = false;
        firstEncounter = false;
    }
    #endregion

    #region Timer Management
    public void CalcTimer()
    {
        remainTime -= Time.deltaTime;
        timerBar.fillAmount = remainTime / maxTime;
    }
    #endregion

    #region Calculation Management
    public void RegenerateAnswer()
    {
        Invoke(nameof(ActivatePanel), 2f);
        calcType = RandomOperator();
        RandomCalculator();
        questionGenerated = true;
        inputhandler.inputField.text = "";
    }

    protected void RandomCalculator()
    {
        operatorSymbol = operators[UnityEngine.Random.Range(0, operators.Length)].ToString();

        firstNumber = UnityEngine.Random.Range(1, 100);
        secondNumber = UnityEngine.Random.Range(1, 10);

        switch (calcType)
        {
            case CalcType.Plus:
                answer = firstNumber + secondNumber;
                questionText.text = $"{firstNumber} + {secondNumber} = ?";
                break;

            case CalcType.Minus:
                answer = firstNumber - secondNumber;
                questionText.text = $"{firstNumber} - {secondNumber} = ?";
                break;

            case CalcType.Multi:
                answer = firstNumber * secondNumber;
                questionText.text = $"{firstNumber} * {secondNumber} = ?";
                break;

            case CalcType.Division:
                answer = Mathf.Round(firstNumber / (float)secondNumber);
                questionText.text = $"{firstNumber} : {secondNumber} = ?";
                break;
        }
    }

    CalcType RandomOperator()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcType));
        System.Random _random = new();
        return (CalcType)_operators.GetValue(_random.Next(_operators.Length));
    }

    public bool CheckAnswer(float playerAnswer)
    {
        return Mathf.Approximately(answer, playerAnswer);
    }

    private void ActivatePanel()
    {
        remainTime = maxTime;
        if (questionPanel != null) questionPanel.SetActive(true);
        if (inputhandler != null)
        {
            inputhandler.playerDealDmg = false;
            inputhandler.monsterDealDmg = false;
        }
    }
    #endregion

    #region Boss Management
    public void OnBossDeath(Enemy boss)
    {
        maxTime += 2;
        isFighting = false;
        player.canMove = true;
        if (calcPanel != null) calcPanel.SetActive(false);
        if (lifePanel != null) lifePanel.SetActive(false);

        inputhandler.UpdateCurrentEnemy();

        if (boss is FirstBoss) StartCoroutine(ActivateNextBoss(secondBoss));
        else if (boss is SecondBoss) StartCoroutine(ActivateNextBoss(thirdBoss));
    }

    private IEnumerator ActivateNextBoss(GameObject nextBoss)
    {
        yield return new WaitForSeconds(1f);
        if (nextBoss != null)
        {
            nextBoss.SetActive(true);
            inputhandler.UpdateCurrentEnemy();
        }
    }
    #endregion
}