using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crescent", menuName = "Phases/Crescent", order = 1)]
public class CrescentMoon : MoonPhase
{
    public float animationSwingSpeedMultiplier = 1.5f;
    public float bonusKnockback = 10;
    public override void PhaseEnter(ref PlayerController c)
    {
        base.PhaseEnter(ref c);

        c.Knockback += bonusKnockback;
        c.animator.SetFloat("AttackSpeed", animationSwingSpeedMultiplier);
    }

    public override void PhaseExit(ref PlayerController c)
    {
        base.PhaseExit(ref c);

        c.Knockback = c.BaseKnockBack;

        c.animator.SetFloat("AttackSpeed", 1);
    }
}
