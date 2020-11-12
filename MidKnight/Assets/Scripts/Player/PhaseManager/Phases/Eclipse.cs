using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Eclipse", menuName = "Phases/Eclipse", order = 7)]
public class Eclipse : MoonPhase
{   //We really shouldn't be doing this because its bad practice but we need to to make things clean
    PhaseManager pm = null;
    public override void PhaseEnter(ref PlayerController c)
    {
        base.PhaseEnter(ref c);
        //Never do this. The PM is not accessible from c but we sneak a way around it.
        if (pm == null)
            pm = c.GetComponent<PhaseManager>();

        Debug.Log("Entering Eclipse Mode");

        for (int i = 0; i < pm.everyMoonPhase.Length; i++)
            if (c.ut.GetKeyValue(pm.everyMoonPhase[i].phaseID))
            {
                pm.everyMoonPhase[i].PhaseEnter(ref c);
                pm.everyMoonPhase[i].OnEnter.Invoke();
            }
    }

    public override void PhaseUpdate(ref PlayerController c)
    {
        for (int i = 0; i < pm.everyMoonPhase.Length; i++)
            if (c.ut.GetKeyValue(pm.everyMoonPhase[i].phaseID))
                pm.everyMoonPhase[i].PhaseUpdate(ref c);
    }

    public override void PhaseExit(ref PlayerController c)
    {
        base.PhaseExit(ref c);
        Debug.Log("Exiting Eclipse Mode");

        for (int i = 0; i < pm.everyMoonPhase.Length; i++)
            if (pm.everyMoonPhase[i].Active)
            {
                pm.everyMoonPhase[i].PhaseExit(ref c);
                pm.everyMoonPhase[i].OnExit.Invoke();
            }
    }
}
