using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCtrl : MonoBehaviour
{
    [HideInInspector] public Player player;
    public int health;
    public int maxHealth;
    public bool dead;
    protected void Awake()
    {
        player = GetComponent<Player>();
    }
    public virtual void Die()
    {
        dead = true;
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void TakeDmg(int _dmg)
    {
        if (dead) return;

        health = Mathf.Max(health - _dmg, maxHealth);
        if (health == 0) Die();
    }
}