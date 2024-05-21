using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseDmg : MonoBehaviour
{
    public void ReceivDmg()
    {
        var _character = GetComponentInParent<Character>();
        _character.receivingDmg = !_character.receivingDmg;
    }
    public void TakeDmg()
    {
        Character _character = GetComponentInParent<Character>();
        var targets = Physics2D.OverlapCircleAll
            (
                _character.atqPos.position,
                _character.atqRange,
                _character.attackLayer
            );
        foreach (var target in targets)
        {
            target.GetComponent<Character>().TakeDmg(_character.atqDmg);
        }
    }
}