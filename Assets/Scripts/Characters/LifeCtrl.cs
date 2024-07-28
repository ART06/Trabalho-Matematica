using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

public class LifeCtrl : MonoBehaviour
{
    [HideInInspector] public Character character;
    [HideInInspector] public Player player;
    [HideInInspector] public Enemy enemy;

    public Animator lifeAnim;

    public int health;
    public int healValue;
    public int maxHealth;
    public bool dead;
    public bool canHeal;

    public bool isShielded;
    public int shield;
    public int shieldValue;
    public int maxShield;
    public bool canShield;

    // Referências para a barra de vida segmentada
    public GameObject healthBarPrefab; // Prefab da barrinha de vida
    public GameObject healthBarContainer; // Container das barrinhas de vida
    private List<GameObject> healthBars = new List<GameObject>(); // Lista das barrinhas de vida

    // Referências para a barra de escudo segmentada
    public GameObject shieldBarPrefab; // Prefab da barrinha de escudo
    public GameObject shieldBarContainer; // Container das barrinhas de escudo
    private List<GameObject> shieldBars = new List<GameObject>(); // Lista das barrinhas de escudo

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GetComponent<Enemy>();
    }

    public virtual void Start()
    {
        isShielded = false;
        health = maxHealth;
        maxShield = maxHealth;
        InitializeHealthBar(maxHealth);
        InitializeShieldBar(maxShield);
    }

    public virtual void Update()
    {
        UpdateHealthBar();
        UpdateShieldBar();
    }

    public virtual void Die()
    {
        dead = true;
        character.Death();
        RemoveAllChildObjects(healthBarContainer);
        RemoveAllChildObjects(shieldBarContainer);
    }

    public virtual void TakeDmg(int _dmg)
    {
        if (dead) return;

        if (isShielded)
        {
            UpdateShieldBar();
            shield = Mathf.Max(shield - _dmg, 0);
        }
        else
        {
            if (!player.isFreeze)
                lifeAnim.SetTrigger("Hurt");

            UpdateHealthBar();
            health = Mathf.Max(health - _dmg, 0);
        }

        if (health == 0)
        {
            Die();
        }
    }

    public virtual void GetHeal(int _heal)
    {
        if (dead) return;

        if (isShielded)
        {
            UpdateShieldBar();
            shield = Mathf.Min(shield + _heal, maxShield);
        }
        else
        {
            UpdateHealthBar();
            health = Mathf.Min(health + _heal, maxHealth);
        }
    }

    // Inicialize a barra de vida segmentada
    public void InitializeHealthBar(int maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealthBar();
    }

    // Inicialize a barra de escudo segmentada
    public void InitializeShieldBar(int maxShield)
    {
        this.maxShield = maxShield;
        UpdateShieldBar();
    }

    // Atualize a barra de vida segmentada
    public void UpdateHealthBar()
    {
        // Remova barrinhas extras se a vida máxima diminuir
        while (healthBars.Count > maxHealth)
        {
            Destroy(healthBars[healthBars.Count - 1]);
            healthBars.RemoveAt(healthBars.Count - 1);
        }

        // Adicione barrinhas se a vida máxima aumentar
        while (healthBars.Count < maxHealth)
        {
            GameObject bar = Instantiate(healthBarPrefab, healthBarContainer.transform);
            healthBars.Add(bar);
        }

        // Atualize a aparência das barrinhas baseando-se na vida atual
        for (int i = 0; i < healthBars.Count; i++)
        {
            if (healthBars[i] != null)
            {
                Image barImage = healthBars[i].GetComponent<Image>();
                if (barImage != null)
                {
                    barImage.color = i < health ? Color.green : Color.red;
                }
            }
        }
    }

    // Atualize a barra de escudo segmentada
    public void UpdateShieldBar()
    {
        // Remova barrinhas extras se o escudo máximo diminuir
        while (shieldBars.Count > maxShield)
        {
            Destroy(shieldBars[shieldBars.Count - 1]);
            shieldBars.RemoveAt(shieldBars.Count - 1);
        }

        // Adicione barrinhas se o escudo máximo aumentar
        while (shieldBars.Count < maxShield)
        {
            GameObject bar = Instantiate(shieldBarPrefab, shieldBarContainer.transform);
            shieldBars.Add(bar);
        }

        // Atualize a aparência das barrinhas baseando-se no escudo atual
        for (int i = 0; i < shieldBars.Count; i++)
        {
            if (shieldBars[i] != null)
            {
                Image barImage = shieldBars[i].GetComponent<Image>();
                if (barImage != null)
                {
                    barImage.fillAmount = i < shield ? 1 : 0;
                }
            }
        }
    }

    // Remova todos os objetos filhos do container especificado, se houver
    private void RemoveAllChildObjects(GameObject container)
    {
        if (container.transform.childCount == 0) return;

        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }
}