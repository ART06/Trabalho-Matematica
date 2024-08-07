using UnityEngine;

public class Character : MonoBehaviour
{
    protected Player player;
    protected Enemy enemy;
    [HideInInspector] public LifeCtrl Life;
    protected InputHandler inputHandler;

    [HideInInspector] public Rigidbody2D rb;
    public Transform atqPos;
    public LayerMask attackLayer;
    public int atqDmg;
    public float atqRange;

    protected CauseDmg CauseDmg;
    [HideInInspector] public bool receivingDmg;

    public Animator anim;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<InputHandler>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        Life = GetComponent<LifeCtrl>();
        CauseDmg = GetComponent<CauseDmg>();
    }
    public virtual void Update()
    {
        Animations();
    }
    public virtual void TakeDmg(int _value)
    {
        if (Life.dead) return;
        Life.TakeDmg(_value);
        if (Life.health == 0) Death();
    }
    public virtual void Death()
    {
        player.direction.x = 0f;
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