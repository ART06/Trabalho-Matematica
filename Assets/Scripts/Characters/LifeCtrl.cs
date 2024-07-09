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
    
    protected bool isShielded;
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
        maxShield = maxHealth;
        shieldBar.gameObject.SetActive(false);
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

        health = Mathf.Max(health - _dmg, 0);
        lifeAnim.SetTrigger("Hurt");

        if (isShielded) shieldBar.fillAmount = (float)shield / maxShield;
        else healthBar.fillAmount = (float)health / maxHealth;

        if (health <= 0) healthBar.fillAmount = 0;
        if (health == 0) Die();
    }
    public virtual void GetHeal(int _heal)
    {
        if (!canHeal || dead) return;

        health = Mathf.Min(health + _heal, maxHealth);
        healthBar.fillAmount = (float)health / maxHealth;
    }
}