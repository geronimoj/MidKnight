using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class StateManager : MonoBehaviour
{
    [SerializeField]
    private State current;
    private State target;
    [Tooltip("These transitions are always checked and cannot be disabled")]
    public Transition[] globalTransitions;
    /// <summary>
    /// Calls the start function of the current state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void CallStart(PlayerController c)
    {
        if (current != null)
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
        else
            Debug.LogWarning("No State Assigned");
    }
    /// <summary>
    /// Swaps to the target state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    private void SwapState(ref PlayerController c)
    {   //If we are already in the state don't both
        if (target == current)
            return;
        current.StateEnd(ref c);

        current = target;

        current.StateStart(ref c);
    }
    /// <summary>
    /// Returns true if any transition returned true. Also assigns target
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <param name="transitions">The transitions to check</param>
    /// <param name="ignore">An optional boolean array for transitions that should be ignored. Can be null</param>
    /// <returns>Returns true if a transition passed. Target will have already been assigned</returns>
    private bool CheckTransitions(ref PlayerController c, ref Transition[] transitions, bool[] ignore)
    {   //Make sure we have transitions to check
        if (transitions == null)
            return false;
        for (int i = 0; i < transitions.Length; i++)
        {   //Should we transition
            if (transitions[i].ShouldTransition(ref c))
            {   //Should this transition be ignored
                if (ignore != null && i < ignore.Length)
                        if (ignore[i])
                            continue;
                
                //Swap target and return true
                target = transitions[i].target;
                return true;
            }
        }
        //No transitions passed
        return false;
    }
}
