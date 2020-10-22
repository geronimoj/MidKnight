using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_ElseTransition : Transition
{
    public State ifState;
    public State elseState;

    public override bool ShouldTransition(ref PlayerController c)
    {
        if (IfTransition(ref c))
            target = ifState;
        else
            target = elseState;

        return true;
    }

    public virtual bool IfTransition(ref PlayerController c)
    {
        Debug.LogError("If Transition not overrided");
        return false;
    }
}
