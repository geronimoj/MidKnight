using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealSpell", menuName = "States/HealSpell", order = 9)]
public class HealSpell : CastSpell
{
    public int healAmount = 0;
    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="c"></param>
    protected override void DoSpell(ref PlayerController c)
    {
        c.animator.SetBool("FinishHeal", true);
        c.MoonLight -= spellCost;
        c.TakeDamage(-healAmount);
    }
    /// <summary>
    /// Make sure the player hasn't taken damage
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    protected override void ExtraUpdate(ref PlayerController c)
    {   //If the player takes damage in the wind up, break out of the heal
        if (c.TookDamageThisFrame())
            ignoreTransitions[0] = false;
    }

    protected override void ExtraOnEnter(ref PlayerController c)
    {   //Enter the heal animation
        c.animator.SetTrigger("DoHeal");
        c.animator.SetBool("FinishHeal", false);
    }
}
