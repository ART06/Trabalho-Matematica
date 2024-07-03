using UnityEngine;

public class Enemy : Character
{
    protected bool enemyTurn;
    protected int randomAction;

    protected void OnBossRound()
    {
        randomAction = Random.Range(1, 3);
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position);
    }
}