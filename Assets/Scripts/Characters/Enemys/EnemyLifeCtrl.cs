using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeCtrl : LifeCtrl
{
    #region Life Control
    public override void Die()
    {
        base.Die();
        enemy.anim.SetBool("Death", true);
        Invoke(nameof(Deactivate), 1f);
    }
    #endregion

    #region Private Methods
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    #endregion
}