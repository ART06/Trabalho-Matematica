using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeCtrl : LifeCtrl
{
    public override void TakeDmg(int _dmg)
    {
        base.TakeDmg(_dmg);
        if (dead) return;
        character.anim.SetTrigger("Hurt");
    }
    public override void Die()
    {
        base.Die();
        GameManager.instance.deathPanel.SetActive(true);
        GameManager.instance.calcPanel.SetActive(false);
        character.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        character.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
