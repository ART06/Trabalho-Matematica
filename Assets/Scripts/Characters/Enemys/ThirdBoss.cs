public class ThirdBoss : Enemy
{
    public bool thirdBoss;

    protected override void Start()
    {
        base.Start();
        thirdBoss = true;
    }
    protected void FixedUpdate()
    {
        if (DistanceToPlayer() <= atqRange && thirdBoss) GameManager.instance.isFighting = true;
    }

    public override void Death()
    {
        base.Death();
        thirdBoss = false;
        GameManager.instance.OnBossDeath(this);
    }
}