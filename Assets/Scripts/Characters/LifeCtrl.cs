using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LifeCtrl : MonoBehaviour
{
    [HideInInspector] public Character character;
    [HideInInspector] public Player player;
    [HideInInspector] public Enemy enemy;
    
    public Image healthBar;
    public Image shieldBar;
    public Animator lifeAnim;

    public int health;
    public int healValue;
    public int maxHealth;
    public bool dead;
    public bool canHeal;
    
    [HideInInspector] public bool isShielded;
    public int shield;
    public int shieldValue;
    public int maxShield;
    public bool canShield;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GetComponent<Enemy>();
        health = maxHealth;
    }
    public virtual void Start()
    {
        isShielded = false;
        shieldBar.gameObject.SetActive(false);
    }
    public void Update()
    {
        shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, (float)shield / maxShield, 0.1f);
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / maxHealth, 0.1f);
    }
    public virtual void Die()
    {
        dead = true;
        enemy.anim.SetBool("Death", true);
        character.Death();
    }
    public virtual void TakeDmg(int _dmg)
    {
        if (dead) return;

        lifeAnim.SetTrigger("Hurt");

        if (isShielded)
        {
            shield = Mathf.Max(shield - _dmg, 0);
        }
        else
        {
            health = Mathf.Max(health - _dmg, 0);
        }

        if (health == 0)
        {
            healthBar.fillAmount = 0;
            Die();
        }
    }
    public virtual void GetHeal(int _heal)
    {
        if (!canHeal || dead) return;

        if (isShielded)
        {
            shield = Mathf.Min(shield + _heal, maxShield);
        }
        else
        {
            health = Mathf.Min(health + _heal, maxHealth);
        }
    }
}