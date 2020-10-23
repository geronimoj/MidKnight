using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_ElseTransition : Transition
{
    /// <summary>
    /// The state to swap to if we pass the IfTransition
    /// </summary>
    public State ifState;
    /// <summary>
    /// The state to swap to if we fail the IfTransition
    /// </summary>
    public State elseState;
    /// <summary>
    /// Don't override this function. Always returns true
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>Always returns true</returns>
    public override bool ShouldTransition(ref PlayerController c)
    {   //Did IfTransition Pass
        if (IfTransition(ref c))
            target = ifState;
        else
            target = elseState;

        return true;
    }
    /// <summary>
    /// An overrideable function for the transition
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>Returns false by default</returns>
    public virtual bool IfTransition(ref PlayerController c)
    {
        Debug.LogError("If Transition not overrided");
        return false;
    }
}
