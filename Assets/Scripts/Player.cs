using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeCtrl))]
public class Player : Character
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector2 direction;
    protected Vector2 imputs;
    protected Vector3 localScale;
    public int speed;

    public bool canMove;
    [HideInInspector] public bool timeToAttack;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }
    protected void Update()
    {
        Flip();
        PlayerImputs();
    }
    protected void FixedUpdate()
    {
        Move();
    }
    protected void Move()
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }
    protected void Flip()
    {
        if ((transform.localScale.x > 0 && rb.velocity.x < -0.2f) ||
            (transform.localScale.x < 0 && rb.velocity.x > 0.2f))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    protected void PlayerImputs()
    {
        if (!canMove)
        {
            direction.x = 0f;
            return;
        }
        else direction.x = Input.GetAxisRaw("Horizontal");
    }
    public override void TakeDmg(int _dmg)
    {
        base.TakeDmg(_dmg);
    }
}