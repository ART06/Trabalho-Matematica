using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public Vector2 direction;
    protected Vector2 imputs;
    protected Vector3 localScale;
    public int speed;

    public bool canMove;
    [HideInInspector] public bool isFreeze;
    public Animator healAnim;

    protected void Start()
    {
        canMove = true;
    }
    public override void Update()
    {
        base.Update();
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
    
    protected override void Animations()
    {
        base.Animations();

        if (direction.x != 0)
            anim.SetFloat("VelocX", 1);
        else
            anim.SetFloat("VelocX", 0);

        if (isFreeze)
            anim.SetBool("IsFreeze", true);
        else
            anim.SetBool("IsFreeze", false);
    }
}