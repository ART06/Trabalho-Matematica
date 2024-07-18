using TMPro;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("Calculation Variables")]
    protected int calcMD;
    protected int calcPM;
    protected int calcSS;
    protected int specialNumbers1;
    protected int specialNumbers2;
    protected int basicNumbers;
    protected int squaredNumber;
    protected int sqrRootNumber;
    protected int preAnswer;
    public int answer;

    [HideInInspector] public string question = "";
    [HideInInspector] public int maxIterations;
    [HideInInspector] public int iterationCount;

    [Header("UI Elements")]
    [SerializeField] internal TextMeshProUGUI questionText;
    public int roundCooldown;
    public int remainCooldown;

    [Header("Game State Variables")]
    [HideInInspector] public bool isOnCooldown;
    public bool canCooldown;

    public virtual void Start()
    {
        isOnCooldown = false;
    }
    public void GenerateAnswer()
    {
        RandomOperatorPM();
        RandomOperatorMD();
        RandomOperatorSS();
    }

    public virtual void ActivateSkill()
    {
        specialNumbers1 = Random.Range(5, 61);
        specialNumbers2 = Random.Range(5, 61);
        basicNumbers = Random.Range(5, 101);

        maxIterations = 1000;
        iterationCount = 0;
        remainCooldown = roundCooldown;

        GenerateAnswer();
    }
    public void FinalAnswer()
    {
        GameManager.instance.correctAnswer = answer;

        if (GameManager.instance.questionPanel != null) GameManager.instance.questionPanel.SetActive(true);
        GameManager.instance.isCalc = true;
        if (GameManager.instance.habilityPanel != null) GameManager.instance.habilityPanel.SetActive(false);
        StartCoroutine(GameManager.instance.ActivatePanel());
    }

    public void RandomOperatorPM()
    {
        calcPM = Random.Range(0, 2);
    }

    public void RandomOperatorMD()
    {
        calcMD = Random.Range(0, 2);
    }

    public void RandomOperatorSS()
    {
        calcSS = Random.Range(0, 2);
    }

    public void UpdateCooldown()
    {
        if (remainCooldown > 0)
        {
            remainCooldown--;
            if (remainCooldown <= 0)
            {
                isOnCooldown = false;
                remainCooldown = 0;
            }
        }
    }
}
