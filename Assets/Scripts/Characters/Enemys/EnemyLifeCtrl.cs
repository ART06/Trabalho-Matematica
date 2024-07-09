using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeCtrl : LifeCtrl
{
    #region Variables

    #endregion

    #region Unity Methods



    #endregion

    #region Life Control
    public override void Die()
    {
        base.Die();
        Invoke(nameof(Deactivate), 1.5f);
    }
    #endregion

    #region Private Methods
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    #endregion
}