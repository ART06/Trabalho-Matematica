﻿using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private readonly int[] falseAnswer = new int[2];

    [HideInInspector] public static GameManager instance;

    [Header("Player and Enemy References")]
    protected Enemy enemy;
    [HideInInspector] public Character character;
    [HideInInspector] public Player player;
    [HideInInspector] public InputHandler inputhandler;
    [HideInInspector] public Skills skills;
    [HideInInspector] public BasicSkill basicSkill;
    [HideInInspector] public HealSkill healSkill;
    [HideInInspector] public AdvancedSkill advancedSkill;

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
    [HideInInspector] public int rightPos;
    [HideInInspector] public int correctAnswer;
    [HideInInspector] public int offset;
    [HideInInspector] public int lastRightPos;

    public float maxTime;
    [HideInInspector] public bool enemyTurn;
    [HideInInspector] public bool playerTurn;

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
        playerTurn = false;
        enemyTurn = false;
        lastRightPos = -1;
    }
    private void FixedUpdate()
    {
        if (isFighting)
        {
            if (falseAnswerIsGenerated)
            {
                Debug.Log("resposta correta: " + correctAnswer);
                RightAnswerPos();
                IncorrectNumberGenerator();
                falseAnswerIsGenerated = false;
            }
            CombatManager();

            if (isCalc)
                CalcTimer();

            UpdateCooldownTexts();
            skills.UpdateCooldown();
            enemy.UpdateCooldown();
        }
    }

    private void InitializeUIElements()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (battlePanel != null)
            battlePanel.SetActive(false);

        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (secondBoss != null)
            secondBoss.SetActive(false);

        if (thirdBoss != null)
            thirdBoss.SetActive(false);
    }

    private void InitializeComponents()
    {
        advancedSkill = GetComponent<AdvancedSkill>();
        inputhandler = GetComponent<InputHandler>();
        basicSkill = GetComponent<BasicSkill>();
        healSkill = GetComponent<HealSkill>();
        skills = GetComponent<Skills>();

        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        sb = FindObjectOfType<SecondBoss>();
        fb = FindObjectOfType<FirstBoss>();
        tb = FindObjectOfType<ThirdBoss>();

        inputhandler.UpdateCurrentEnemy();
    }

    private void SetupButtonListeners()
    {
        if (button1 != null)
            button1.onClick.AddListener(() => inputhandler.ValidateInput(textButton1.text));

        if (button2 != null)
            button2.onClick.AddListener(() => inputhandler.ValidateInput(textButton2.text));

        if (button3 != null)
            button3.onClick.AddListener(() => inputhandler.ValidateInput(textButton3.text));
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
        }

        if (player.IsDead())
            battlePanel.SetActive(false);
    }

    private void ActivateCombatUI()
    {
        if (battlePanel != null)
            battlePanel.SetActive(true);

        if (battleUI != null)
            battleUI.SetActive(true);

        playerTurn = true;
    }

    private void ResetCombatState()
    {
        habilityPanel.SetActive(true);
        remainTime = maxTime;
        enemyTurn = false;
        battlePanel.SetActive(false);
        enemy.Life.healthBar.fillAmount = 1f;
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
            offset = Random.Range(10, 101);

            if (Random.value > 0.5f)
                falseAnswer[i] = (int)correctAnswer + offset;
            else
                falseAnswer[i] = (int)correctAnswer - offset;

            if (falseAnswer[i] == correctAnswer)
                falseAnswer[i] += offset;

            if (i >= 2) break;
        }
        falseAnswerIsGenerated = true;
    }

    public void RightAnswerPos()
    {
        if (rightPos == 0)
        {
            if (textButton1 != null)
                textButton1.text = correctAnswer.ToString();
            if (textButton2 != null)
                textButton2.text = falseAnswer[0].ToString();
            if (textButton3 != null)
                textButton3.text = falseAnswer[1].ToString();
        }
        else if (rightPos == 1)
        {
            if (textButton1 != null)
                textButton1.text = falseAnswer[0].ToString();
            if (textButton2 != null)
                textButton2.text = correctAnswer.ToString();
            if (textButton3 != null)
                textButton3.text = falseAnswer[1].ToString();
        }
        else
        {
            if (textButton1 != null)
                textButton1.text = falseAnswer[0].ToString();
            if (textButton2 != null)
                textButton2.text = falseAnswer[1].ToString();
            if (textButton3 != null)
                textButton3.text = correctAnswer.ToString();
        }
    }
    public IEnumerator ActivatePanel()
    {
        remainTime = maxTime + 0.2f;
        
        do rightPos = Random.Range(0, 3);
        while (rightPos == lastRightPos);
        lastRightPos = rightPos;

        if (battlePanel != null)
            battlePanel.SetActive(true);
        
        yield return new WaitForSeconds(0.2f);
        if (GameManager.instance.questionPanel != null) GameManager.instance.questionPanel.SetActive(true);
    }

    public void OnBossDeath(Enemy boss)
    {
        maxTime += 2;
        isFighting = false;
        player.canMove = true;
        battlePanel.SetActive(false);
        playerTurn = false;
        enemyTurn = false;

        inputhandler.UpdateCurrentEnemy();

        if (boss is FirstBoss)
            StartCoroutine(ActivateNextBoss(secondBoss));
        else if (boss is SecondBoss)
            StartCoroutine(ActivateNextBoss(thirdBoss));
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

        if (enemy != null && enemy.cooldownText != null)
            enemy.cooldownText.text = enemy.remainCooldown > 0 ? enemy.remainCooldown.ToString() : "";
    }

    public void OnTurnEnd()
    {
        playerTurn = false;
        healSkill.OnTurnEnd();
        advancedSkill.OnTurnEnd();
    }
}