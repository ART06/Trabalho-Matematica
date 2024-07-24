using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLifeCtrl : LifeCtrl
{
    #region Variables
    public SpriteRenderer sprite;
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    public override void Update()
    {
        base.Update();
        if (dead && Input.anyKeyDown) SceneManager.LoadScene(1);
    }
    #endregion

    #region Life Control
    public override void TakeDmg(int _dmg)
    {
        base.TakeDmg(_dmg);
        if (dead || player.isFreeze) return;
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
        if (player.isFreeze) yield return null;

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