using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeCtrl : LifeCtrl
{
    #region Variables
    public SpriteRenderer sprite;
    public Image playerHealthBar;
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Life Control
    public override void TakeDmg(int _dmg)
    {
        base.TakeDmg(_dmg);
        if (dead) return;
        playerHealthBar.fillAmount = (float)health / maxHealth;
        character.anim.SetTrigger("Hurt");
        StartCoroutine(nameof(DmgEffect));
    }

    public override void Die()
    {
        base.Die();
        GameManager.instance.deathPanel.SetActive(true);
        character.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        character.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    #endregion

    #region Private Methods
    IEnumerator DmgEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion
}