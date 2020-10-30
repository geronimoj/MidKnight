using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "States/EmptyState", order = 0)]
public class State : ScriptableObject
{
    /// <summary>
    /// The transitions this state should check
    /// </summary>
    [Tooltip("Checked in order from first to last")]
    public Transition[] transitions;
    /// <summary>
    /// Any transitions that should be ignored
    /// </summary>
    [Tooltip("Set corresponding index to true for transitions on this state that should be ignored")]
    public bool[] ignoreTransitions;

    /// <summary>
    /// Called when the state is entered
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void StateStart(ref PlayerController c) { }
    /// <summary>
    /// Called while the state is active
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void StateUpdate(ref PlayerController c)
    {
        Debug.LogWarning("Transitioned into empty state");
    }
    /// <summary>
    /// Called when the state is exited
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void StateEnd(ref PlayerController c){}
}
