using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeCtrl : LifeCtrl
{
    #region Variables
    public Image enemyHealthBar;
    #endregion

    #region Unity Methods
    public void Update()
    {
        enemyHealthBar.fillAmount = (float)health / maxHealth;
    }
    #endregion

    #region Life Control
    public override void Die()
    {
        base.Die();
        enemy.anim.SetBool("Death", true);
        Invoke(nameof(Deactivate), 0.5f);
    }

    public override void TakeDmg(int _value)
    {
        base.TakeDmg(_value);
        enemy.anim.SetTrigger("Hurt");
    }
    #endregion

    #region Private Methods
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    #endregion
}