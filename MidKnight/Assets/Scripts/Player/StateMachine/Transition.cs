using UnityEngine;

[CreateAssetMenu(fileName = "Transition", menuName = "Transitions/ReturnTrue", order = 0)]
public class Transition : ScriptableObject
{
    /// <summary>
    /// The state to swap too when this transition returns true
    /// </summary>
    public State target;
    /// <summary>
    /// The condition for this state
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>When returns true, swap to target state</returns>
    public virtual bool ShouldTransition(ref PlayerController c)
    {
        return true;
    }
}
