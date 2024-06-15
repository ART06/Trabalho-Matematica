using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] falseAnswer = new int[2];

    public static GameManager instance;

    [Header("Player and Enemy References")]
    [HideInInspector] public Player player;
    protected Enemy enemy;
    protected InputHandler inputhandler;
    protected Character character;
    [HideInInspector] public Skills skills;
    [HideInInspector] public BasicSkill basicSkill;
    [HideInInspector] public HealSkill healSkill;
    [HideInInspector] public AdvancedSkill advancedSkill;

    [Header("Boss References")]
    [HideInInspector] public FirstBoss fb;
    [HideInInspector] public SecondBoss sb;
    [HideInInspector] public ThirdBoss tb;

    [Header("Game State Variables")]
    public bool isFighting;
    public float remainTime;
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
        else Destroy(gameObject);
    }

    private void Start()
    {
        remainTime = maxTime;

        InitializeUIElements();

        player = FindObjectOfType<Player>();
        inputhandler = GetComponent<InputHandler>();
        skills = GetComponent<Skills>();
        basicSkill = GetComponent<BasicSkill>();
        healSkill = GetComponent<HealSkill>();
        advancedSkill = GetComponent<AdvancedSkill>();
        fb = FindObjectOfType<FirstBoss>();
        sb = FindObjectOfType<SecondBoss>();
        tb = FindObjectOfType<ThirdBoss>();

        inputhandler.UpdateCurrentEnemy();
        enemy = inputhandler.GetCurrentEnemy();

        button1.onClick.AddListener(() => inputhandler.ValidateInput(textButton1.text));
        button2.onClick.AddListener(() => inputhandler.ValidateInput(textButton2.text));
        button3.onClick.AddListener(() => inputhandler.ValidateInput(textButton3.text));
    }

    private void InitializeUIElements()
    {
        if (questionPanel != null) questionPanel.SetActive(false);
        if (battlePanel != null) battlePanel.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
        if (secondBoss != null) secondBoss.SetActive(false);
        if (thirdBoss != null) thirdBoss.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isFighting)
        {
            CombatManager();
            if (questionPanel.activeSelf) CalcTimer();
        }
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
        }
        if (player.IsDead() == true) questionPanel.SetActive(false);
    }
    private void ActivateCombatUI()
    {
        if (battlePanel != null) battlePanel.SetActive(true);
        if (battleUI != null) battleUI.SetActive(true);
    }
    private void ResetCombatState()
    {
        remainTime = maxTime;
        isTransitioning = true;

        if (battlePanel != null) battlePanel.SetActive(false);
        if (battleUI != null) battleUI.SetActive(false);
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
            int offset = Random.Range(1, 10);
            if (Random.value > 0.5f)
            {
                falseAnswer[i] = skills.answer + offset;
            }
            else
            {
                falseAnswer[i] = skills.answer - offset;
            }

            if (falseAnswer[i] == skills.answer)
            {
                falseAnswer[i] += offset;
            }
        }
    }
    public void RightAnswerPos()
    {
        int rightPos = Random.Range(0, 3);
        if (rightPos == 0)
        {
            textButton1.text = skills.answer.ToString();
            textButton2.text = falseAnswer[0].ToString();
            textButton3.text = falseAnswer[1].ToString();
        }
        else if (rightPos == 1)
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
        if (questionPanel != null) questionPanel.SetActive(false);
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
}