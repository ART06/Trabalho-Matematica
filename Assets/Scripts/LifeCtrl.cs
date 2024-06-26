using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCtrl : MonoBehaviour
{
    [HideInInspector] public Character character;
    [HideInInspector] public Player player;
    [HideInInspector] public Enemy enemy;
    public int health;
    public int healValue;
    public int maxHealth;
    public bool dead;
    public bool canHeal;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GetComponent<Enemy>();
        health = maxHealth;
    }
    public virtual void Start()
    {

    }
    public virtual void Die()
    {
        dead = true;
        character.Death();
    }
    public virtual void TakeDmg(int _dmg)
    {
        if (dead) return;
        health = Mathf.Max(health - _dmg, 0);
        if (health == 0) Die();
    }
    public virtual void GetHeal(int _heal)
    {
        if (!canHeal || dead) return;
        health = Mathf.Min(health + _heal, maxHealth);
    }
}