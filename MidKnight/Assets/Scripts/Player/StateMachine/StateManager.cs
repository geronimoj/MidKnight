using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class StateManager : MonoBehaviour
{
    public State current;
    private State target;
    [Tooltip("These transitions are always checked and cannot be disabled")]
    public Transition[] globalTransitions;
    /// <summary>
    /// Calls the start function of the current state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void CallStart(PlayerController c)
    {
        current.StateStart(ref c);
    }
    /// <summary>
    /// Checks the transitions, swaps to a different state if necessary, and calls update on the current state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void DoState(PlayerController c)
    {   //Don't do anything if we were given null
        if (c == null)
            return;
        //Check the global transitions                              //Check the states transitions
        if (CheckTransitions(ref c, ref globalTransitions, null) || current != null && CheckTransitions(ref c, ref current.transitions, current.ignoreTransitions))
            //Swap states
            SwapState(ref c);
        //Call update if current exists
        if (current != null)
            current.StateUpdate(ref c);
    }
    /// <summary>
    /// Swaps to the target state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    private void SwapState(ref PlayerController c)
    {
        current.StateEnd(ref c);

        current = target;

        current.StateStart(ref c);
    }

    private bool CheckTransitions(ref PlayerController c, ref Transition[] transitions, bool[] ignore)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].ShouldTransition(ref c))
            {
                if (ignore != null && ignore.Length < i && ignore[i])
                    continue;

                target = transitions[i].target;
                return true;
            }
        }

        return false;
    }
}
