using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Enums
    public CalcTypeMD calcTypeMD;
    public CalcTypePM calcTypePM;
    public CalcTypeSS calcTypeSS;
    public enum CalcTypeMD { Multi, Division }
    public enum CalcTypePM { Plus, Minus }
    public enum CalcTypeSS { SqrRoot, Squared }

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
    protected int specialNumbers1;
    protected int specialNumbers2;
    protected int basicNumbers;
    protected int squaredNumber;
    protected int sqrRootNumber;
    public float remainTime;
    public float maxTime;
    public float answer;
    protected float preAnswer;

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
        enemy = inputhandler.GetCurrentEnemy();
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
        inputhandler.UpdateCurrentEnemy();
        enemy = inputhandler.GetCurrentEnemy();

        calcTypePM = RandomOperator1();
        calcTypeMD = RandomOperator2();
        calcTypeSS = RandomOperator3();
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

        if (calcPanel != null) calcPanel.SetActive(false);
        if (lifePanel != null) lifePanel.SetActive(false);
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
        calcTypePM = RandomOperator1();
        calcTypeMD = RandomOperator2();
        calcTypeSS = RandomOperator3();
        RandomCalculator();
        questionGenerated = true;
        inputhandler.inputField.text = "";
    }

    protected void RandomCalculator()
    {
        specialNumbers1 = UnityEngine.Random.Range(1, 60);
        specialNumbers2 = UnityEngine.Random.Range(1, 60);
        basicNumbers = UnityEngine.Random.Range(1, 100);
        sqrRootNumber = UnityEngine.Random.Range(1, 100);
        squaredNumber = UnityEngine.Random.Range(1, 30);

        string _question = "";

        if (enemy is FirstBoss)
        {
            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    preAnswer = specialNumbers1 * specialNumbers2;
                    _question += $"{specialNumbers1} * {specialNumbers2}";
                    break;

                case CalcTypeMD.Division:
                    while (true)
                    {
                        if (specialNumbers1 % specialNumbers2 == 0)
                        {
                            preAnswer = specialNumbers1 / specialNumbers2;
                            _question += $"{specialNumbers1} : {specialNumbers2}";
                            break;
                        }
                    }
                    break;
            }
            switch (calcTypePM)
            {
                case CalcTypePM.Plus:
                    answer = preAnswer + basicNumbers;
                    questionText.text = $"{_question} + {basicNumbers} = ?";
                    break;

                case CalcTypePM.Minus:
                    answer = preAnswer - basicNumbers;
                    questionText.text = $"{_question} - {basicNumbers} = ?";
                    break;
            }
            
        }
        else if (enemy is SecondBoss)
        {
            specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
            _question += $"{squaredNumber}²";

            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    preAnswer = specialNumbers1 * specialNumbers2;
                    _question += $" * {specialNumbers2}";
                    break;

                case CalcTypeMD.Division:
                    while (true)
                    {
                        if (specialNumbers1 % specialNumbers2 == 0)
                        {
                            preAnswer = specialNumbers1 / specialNumbers2;
                            _question += $" : {specialNumbers2}";
                            break;
                        }
                    }
                    break;
            }
            switch (calcTypePM)
            {
                case CalcTypePM.Plus:
                    answer = preAnswer + basicNumbers;
                    questionText.text = $"{_question} + {basicNumbers} = ?";
                    break;

                case CalcTypePM.Minus:
                    answer = preAnswer - basicNumbers;
                    questionText.text = $"{_question} - {basicNumbers} = ?";
                    break;
            }
        }
        else if (enemy is ThirdBoss)
        {
            switch (calcTypeSS)
            {
                case CalcTypeSS.SqrRoot:
                    while (true)
                    {
                        sqrRootNumber = (int)Mathf.Sqrt(sqrRootNumber);
                        if (sqrRootNumber == Mathf.Floor(sqrRootNumber))
                        {
                            specialNumbers1 = sqrRootNumber;
                            _question += $"√{sqrRootNumber}";
                            break;
                        }
                    }
                    break;

                case CalcTypeSS.Squared:
                    specialNumbers1 = (int)Mathf.Pow(squaredNumber, 2);
                    _question += $"{squaredNumber}²";
                    break;
            }
            switch (calcTypeMD)
            {
                case CalcTypeMD.Multi:
                    _question += $" *";
                    break;

                case CalcTypeMD.Division:
                    _question += $" :";
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
                            _question += $" √{sqrRootNumber}";
                            break;
                        }
                    }
                    break;

                case CalcTypeSS.Squared:
                    specialNumbers2 = (int)Mathf.Pow(squaredNumber, 2);
                    _question += $" {squaredNumber}²";
                    break;
            }
            if (calcTypeMD == CalcTypeMD.Multi)
            {
                preAnswer = specialNumbers1 * specialNumbers2;
                _question += $" {specialNumbers2}";
            }
            else if (calcTypeMD == CalcTypeMD.Division)
            {
                while (true)
                {
                    if (specialNumbers1 % specialNumbers2 == 0)
                    {
                        preAnswer = specialNumbers1 / specialNumbers2;
                        _question += $" {specialNumbers2}";
                        break;
                    }
                }
            }
            switch (calcTypePM)
            {
                case CalcTypePM.Plus:
                    answer = preAnswer + basicNumbers;
                    questionText.text = $"{_question} + {basicNumbers} = ?";
                    break;
                case CalcTypePM.Minus:
                    answer = preAnswer - basicNumbers;
                    questionText.text = $"{_question} - {basicNumbers} = ?";
                    break;
            }
        }
    }
    CalcTypePM RandomOperator1()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypePM));
        System.Random _random = new();
        return (CalcTypePM)_operators.GetValue(_random.Next(_operators.Length));
    }
    CalcTypeMD RandomOperator2()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypeMD));
        System.Random _random = new();
        return (CalcTypeMD)_operators.GetValue(_random.Next(_operators.Length));
    }
    CalcTypeSS RandomOperator3()
    {
        System.Array _operators = System.Enum.GetValues(typeof(CalcTypeSS));
        System.Random _random = new();
        return (CalcTypeSS)_operators.GetValue(_random.Next(_operators.Length));
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