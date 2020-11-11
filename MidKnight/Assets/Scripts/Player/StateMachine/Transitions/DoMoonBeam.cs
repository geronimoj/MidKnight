using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DoMoonBeam", menuName = "Transitions/DoMoonBeam", order = 11)]
public class DoMoonBeam : DoSpell
{
    public override bool ShouldTransition(ref PlayerController c)
    {
        if (base.ShouldTransition(ref c))
        {
            if (c.CanCastMoonBeam && c.ut.GetKeyValue("Moon Beam"))
                return true;
            else
                return false;
        }

        return false;
    }
}
