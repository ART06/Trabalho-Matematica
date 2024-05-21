using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCtrl : MonoBehaviour
{
    [HideInInspector] public Player player;
    public int health;
    public int maxHealth;
    public bool dead;
    public Image healthBar;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        health = maxHealth;
    }
    public virtual void Die()
    {
        dead = true;
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public virtual void TakeDmg(int _dmg)
    {
        if (dead) return;

        healthBar.fillAmount = health / 100f;

        health = Mathf.Max(health - _dmg, maxHealth);

        if (health == 0) Die();
    }
}