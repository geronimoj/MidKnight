using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class PhaseManager : MonoBehaviour
{
    /// <summary>
    /// The minimum time you must remain in a phase before you can swap again
    /// </summary>
    public float swapCooldown = 0;
    /// <summary>
    /// The timer for the swapCooldown
    /// </summary>
    private float cooldownTimer = 0;
    /// <summary>
    /// A storage location for the current moonPhase. Is viewable in the inspector
    /// </summary>
    [SerializeField]
    private MoonPhase current;

    private List<MoonPhase> knownPhases;
    /// <summary>
    /// Called when the phase is swapped
    /// </summary>
    public UnityEvent OnSwap;
    /// <summary>
    /// A get for the current phase
    /// </summary>
    public MoonPhase CurrentPhase
    {
        get
        {
            return current;
        }
    }

    public void Start()
    {
        knownPhases = new List<MoonPhase>();
        if (current != null)
            knownPhases.Add(current);
    }

    /// <summary>
    /// Calls update on the current phase if there is one
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void PhaseUpdate(PlayerController c)
    {
        cooldownTimer -= Time.deltaTime;
        DecrementTimers();
        if (current != null)
            current.DoPhase(ref c);
#if UNITY_EDITOR
        else
            Debug.LogWarning("No State");
#endif
    }
    /// <summary>
    /// Calls Start on the current phase. Primarily used when first initialising an object
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void PhaseStart(PlayerController c)
    {
        if (current != null)
        {
            current.PhaseEnter(ref c);
            current.OnEnter.Invoke();
        }
    }
    /// <summary>
    /// Swaps the phase to target if possible
    /// </summary>
    /// <param name="target">The phase to swap to</param>
    /// <param name="c">A reference to the player controller</param>
    public void SwapPhase(MoonPhase target, PlayerController c)
    {
        if (cooldownTimer > 0 || target == null || target.OnCooldown)
            return;
        cooldownTimer = swapCooldown;
        current.PhaseExit(ref c);
        current.OnExit.Invoke();

        current = target;
        OnSwap.Invoke();

        current.PhaseEnter(ref c);
        current.OnEnter.Invoke();
        //Add the phase to the list of known phases if its not already there
        if (!knownPhases.Contains(current))
            knownPhases.Add(current);
    }
    /// <summary>
    /// Returns true if the string given matches the phase ID of the current phase
    /// </summary>
    /// <param name="phaseID">The ID to compare against</param>
    /// <returns>Returns true if the string matches the current phases ID</returns>
    public bool CorrectPhase(string phaseID)
    {
        return current.phaseID.Equals(phaseID);
    }

    private void DecrementTimers()
    {
        for (int i = 0; i < knownPhases.Count; i++)
            knownPhases[i].DecrementCooldownTimer();
    }
#if UNITY_EDITOR
    public Vector3 test;
    public Vector3 test2;
    /// <summary>
    /// Draws the PhaseAttack hitboxes
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + (transform.right * test.x) + (transform.forward * test.z) + transform.up * test.y, 1f);
        Gizmos.DrawSphere(transform.position + (transform.right * (test.x + test2.x)) + (transform.forward * (test.z + test2.z)) + transform.up * (test.y + test2.y), 1f);
    }
#endif
}
