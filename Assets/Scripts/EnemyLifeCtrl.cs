using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeCtrl : LifeCtrl
{
    public override void Die()
    {
        base.Die();
        enemy.anim.GetBool("Death");
        enemy.isFightingPlayer = false;
        Invoke(nameof(Deactivate), 0.5f);
    }
    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);
        enemy.anim.SetTrigger("Hurt");
        //ELifeCtrlSoundManager.PlaySFX(ELifeCtrlSoundManager.enemyHit);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}