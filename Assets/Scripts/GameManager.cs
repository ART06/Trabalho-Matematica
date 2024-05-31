using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public enum CalcType { Plus, Minus, Multi, Division };
    public CalcType calcType;

    public static GameManager instance;
    protected Player player;
    protected Enemy enemy;
    protected InputHandler inputhandler;
    protected Character character;
    [HideInInspector] public FirstBoss fb;
    [HideInInspector] public SecondBoss sb;
    [HideInInspector] public ThirdBoss tb;

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
    public GameObject firstBoss;
    public GameObject secondBoss;
    public GameObject thirdBoss;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI asnwerVerifyText;

    private bool isTransitioning;

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
        if (calcPanel != null) calcPanel.SetActive(false);
        if (lifePanel != null) lifePanel.SetActive(false);
        if (deathPanel != null) deathPanel.SetActive(false);
        if (secondBoss != null) secondBoss.SetActive(false);
        if (thirdBoss != null) thirdBoss.SetActive(false);
        if (asnwerVerifyText != null) asnwerVerifyText.gameObject.SetActive(false);

        player = FindObjectOfType<Player>();
        inputhandler = GetComponent<InputHandler>();
        fb = FindObjectOfType<FirstBoss>();
        sb = FindObjectOfType<SecondBoss>();
        tb = FindObjectOfType<ThirdBoss>();
    }

    private void FixedUpdate()
    {
        CombatManager();
        if (inputhandler != null) inputhandler.ValidateInput();
        Debug.Log($"Está em luta?: {isFighting}");
    }

    protected void CombatManager()
    {
        if (isFighting)
        {
            if (calcPanel != null) calcPanel.SetActive(true);
            if (lifePanel != null) lifePanel.SetActive(true);

            if (!questionGenerated && !firstEncounter)
            {
                calcType = RandomOperator();
                RandomCalculator();
                questionGenerated = true;
                firstEncounter = true;
            }
            Debug.Log($"Resposta gerada: {answer}");
            player.canMove = false;
        }
        else
        {
            isTransitioning = true;
            inputhandler.inputField.text = "";
            questionGenerated = false;
            firstEncounter = false;
        }
        if (player.IsDead() == true) calcPanel.SetActive(false);
    }

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
        if (questionPanel != null)
        {
            questionPanel.SetActive(true);
        }

        if (inputhandler != null)
        {
            inputhandler.playerDealDmg = false;
            inputhandler.monsterDealDmg = false;
        }
    }

    public void OnBossDeath(Enemy boss)
    {
        isFighting = false;
        player.canMove = true;
        if (calcPanel != null) calcPanel.SetActive(false);
        if (lifePanel != null) lifePanel.SetActive(false);

        // Atualiza o inimigo atual após a morte do chefe
        inputhandler.UpdateCurrentEnemy();

        if (boss is FirstBoss) StartCoroutine(ActivateNextBoss(secondBoss));
        else if (boss is SecondBoss) StartCoroutine(ActivateNextBoss(thirdBoss));
    }

    private IEnumerator ActivateNextBoss(GameObject nextBoss)
    {
        yield return new WaitForSeconds(1f); // Ajuste o tempo conforme necessário
        if (nextBoss != null)
        {
            nextBoss.SetActive(true);
            // Atualiza o inimigo atual após ativar o próximo chefe
            inputhandler.UpdateCurrentEnemy();
        }
    }
}
