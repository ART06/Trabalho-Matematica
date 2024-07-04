using UnityEngine;

public class Enemy : Character
{
    protected int randomAction;
    protected InputHandler input;
    public Animator critAnim;
    public Animator specAnim;

    protected virtual void Start()
    {
        input = GetComponent<InputHandler>();
    }
    public virtual void BossRound()
    {
        if (!enemyTurn) return;

        randomAction = Random.Range(1, 3);
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
}