using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] falseAnswer = new int[2];

    public static GameManager instance;

    [Header("Player and Enemy References")]
    protected Enemy enemy;
    [HideInInspector] public Character character;
    [HideInInspector] public Player player;
    [HideInInspector] public InputHandler inputhandler;
    [HideInInspector] public Skills skills;
    public BasicSkill basicSkill;
    public HealSkill healSkill;
    public AdvancedSkill advancedSkill;

    [Header("Boss References")]
    [HideInInspector] public FirstBoss fb;
    [HideInInspector] public SecondBoss sb;
    [HideInInspector] public ThirdBoss tb;

    [Header("Game State Variables")]
    [HideInInspector] public bool isFighting;
    [HideInInspector] public bool isCalc;
    [HideInInspector] public bool falseAnswerIsGenerated;
    [HideInInspector] public bool answerIsGenerated;
    [HideInInspector] public float remainTime;
    public float maxTime;

    [Header("UI Elements")]
    public Image timerBar;
    public GameObject questionPanel;
    public GameObject battleUI;
    public GameObject habilityPanel;
    public GameObject lifePanel;
    public GameObject battlePanel;
    public GameObject deathPanel;
    public GameObject firstBoss;
    public GameObject secondBoss;
    public GameObject thirdBoss;
    public TextMeshProUGUI questionText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Text textButton1;
    public Text textButton2;
    public Text textButton3;

    protected bool isTransitioning;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeUIElements();
        InitializeComponents();
        SetupButtonListeners();
    }

    private void Start()
    {
        remainTime = maxTime;
        isCalc = false;
    }
    private void FixedUpdate()
    {
        if (isFighting)
        {
            if (falseAnswerIsGenerated) IncorrectNumberGenerator();
            CombatManager();
            if (isCalc)
            {
                RightAnswerPos();
                CalcTimer();
            }

            UpdateCooldownTexts();
            if (skills != null)
                skills.UpdateCooldown();
        }
    }
    private void InitializeUIElements()
    {
        if (questionPanel != null) questionPanel.SetActive(false);
        if (battlePanel != null) battlePanel.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
        if (secondBoss != null) secondBoss.SetActive(false);
        if (thirdBoss != null) thirdBoss.SetActive(false);
    }
    private void InitializeComponents()
    {
        advancedSkill = GetComponent<AdvancedSkill>();
        inputhandler = GetComponent<InputHandler>();
        basicSkill = GetComponent<BasicSkill>();
        healSkill = GetComponent<HealSkill>();
        skills = GetComponent<Skills>();

        player = FindObjectOfType<Player>();
        sb = FindObjectOfType<SecondBoss>();
        fb = FindObjectOfType<FirstBoss>();
        tb = FindObjectOfType<ThirdBoss>();

        enemy = inputhandler.GetCurrentEnemy();
        inputhandler.UpdateCurrentEnemy();

        if (advancedSkill == null) Debug.LogError("AdvancedSkill is not assigned in GameManager");
        if (inputhandler == null) Debug.LogError("InputHandler is not assigned in GameManager");
        if (basicSkill == null) Debug.LogError("BasicSkill is not assigned in GameManager");
        if (healSkill == null) Debug.LogError("HealSkill is not assigned in GameManager");
        if (skills == null) Debug.LogError("Skills is not assigned in GameManager");
        if (player == null) Debug.LogError("Player is not assigned in GameManager");
    }

    private void SetupButtonListeners()
    {
        if (button1 != null) button1.onClick.AddListener(() => inputhandler.ValidateInput(textButton1.text));
        if (button2 != null) button2.onClick.AddListener(() => inputhandler.ValidateInput(textButton2.text));
        if (button3 != null) button3.onClick.AddListener(() => inputhandler.ValidateInput(textButton3.text));
    }
    private void CombatManager()
    {
        if (isFighting)
        {
            ActivateCombatUI();
            player.canMove = false;
        }
        else
        {
            ResetCombatState();
            falseAnswerIsGenerated = false;
            skills.remainCooldown = 0;
        }
        if (player.IsDead()) battlePanel.SetActive(false);
    }
    private void ActivateCombatUI()
    {
        if (battlePanel != null) battlePanel.SetActive(true);
        if (battleUI != null) battleUI.SetActive(true);
        if (character != null) character.playerTurn = true;
    }
    private void ResetCombatState()
    {
        remainTime = maxTime;

        skills.isOnCooldown = false;
        skills.remainCooldown = 0;

        isTransitioning = true;

        if (battlePanel != null) battlePanel.SetActive(false);
    }
    public void CalcTimer()
    {
        remainTime -= Time.deltaTime;
        timerBar.fillAmount = remainTime / maxTime;
    }
    public void IncorrectNumberGenerator()
    {
        for (int i = 0; i < falseAnswer.Length; i++)
        {
            skills.offset = Random.Range(50, 150);

            if (Random.value > 0.5f)
                falseAnswer[i] = (int)skills.answer + skills.offset;
            else
                falseAnswer[i] = (int)skills.answer - skills.offset;

            if (falseAnswer[i] == skills.answer)
                falseAnswer[i] += skills.offset;

            if (i >= 2)
            {
                falseAnswerIsGenerated = false;
                break;
            }
        }
    }
    public void RightAnswerPos()
    {
        if (skills.rightPos == 0)
        {
            textButton1.text = skills.answer.ToString();
            textButton2.text = falseAnswer[0].ToString();
            textButton3.text = falseAnswer[1].ToString();
        }
        else if (skills.rightPos == 1)
        {
            textButton1.text = falseAnswer[0].ToString();
            textButton2.text = skills.answer.ToString();
            textButton3.text = falseAnswer[1].ToString();
        }
        else
        {
            textButton1.text = falseAnswer[0].ToString();
            textButton2.text = falseAnswer[1].ToString();
            textButton3.text = skills.answer.ToString();
        }
    }
    public void ActivatePanel()
    {
        remainTime = maxTime;
        if (battlePanel != null) battlePanel.SetActive(true);
        if (inputhandler != null)
        {
            inputhandler.playerDealDmg = false;
            inputhandler.monsterDealDmg = false;
        }
        IncorrectNumberGenerator();
        RightAnswerPos();
    }

    public void OnBossDeath(Enemy boss)
    {
        maxTime += 2;
        isFighting = false;
        player.canMove = true;
        if (battlePanel != null) battlePanel.SetActive(false);
        if (character != null) character.playerTurn = false;
        if (character != null) character.enemyTurn = false;

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
    public void UpdateCooldownTexts()
    {
        if (advancedSkill != null && advancedSkill.cooldownText != null)
            advancedSkill.cooldownText.text = advancedSkill.remainCooldown > 0 ? advancedSkill.remainCooldown.ToString() : "";

        if (healSkill != null && healSkill.cooldownText != null)
            healSkill.cooldownText.text = healSkill.remainCooldown > 0 ? healSkill.remainCooldown.ToString() : "";
    }

    public void OnTurnEnd()
    {
        if (character != null) character.playerTurn = false;
        if (character != null) character.enemyTurn = true;
        skills.OnTurnEnd();
    }
}