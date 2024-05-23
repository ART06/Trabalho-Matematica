using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Player player;
    protected Enemy enemy;
    protected LifeCtrl Life;
    protected InputHandler inputHandler;

    public Transform atqPos;
    public LayerMask attackLayer;
    public int atqDmg;
    public float atqRange;

    protected CauseDmg CauseDmg;
    [HideInInspector] public bool receivingDmg;

    public Animator anim;

    protected void Awake() 
    {
        inputHandler = GetComponent<InputHandler>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        Life = GetComponent<LifeCtrl>();
        CauseDmg = GetComponent<CauseDmg>();
    }
    protected virtual void Update()
    {
        Animations();
        //DealDmg();
    }
    public virtual void TakeDmg(int _value)
    {
        if (Life.dead) return;
        TakeDmg(_value);
        if (Life.health == 0) Death();
    }
    protected virtual void DealDmg()
    {

    }
    public virtual void Death()
    {

    }
    public bool IsDead()
    {
        return Life.dead;
    }
    protected virtual void Animations()
    {
        anim.SetBool("Death", Life.dead);
    }
}