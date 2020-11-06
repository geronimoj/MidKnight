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
    {   //Timer for the OnDrawGizmos
#if UNITY_EDITOR
        timer += Time.deltaTime;
        if (timer > current.Attacks.attacks[attackIndexToDisplay].duration)
            timer = 0;
#endif
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
    private float timer = 0;
    [Range(0,2)]
    public int attackIndexToDisplay;
    /// <summary>
    /// Draws the PhaseAttack hitboxes
    /// </summary>
    private void OnDrawGizmos()
    {
        if (current == null || current.Attacks == null || current.Attacks.attacks.Length < 1 || current.Attacks.attacks[attackIndexToDisplay] == null)
            return;
        Attack a = current.Attacks.attacks[attackIndexToDisplay];

        Gizmos.color = Color.green;
        
        for (int i = 0; i < a.hitboxes.Length; i++)
        {
            if (!a.GetHitBoxInfo(a.hitboxes[i], transform, timer, Time.deltaTime, out Vector3 origin, out Vector3 secOrigin, out Vector3 dir, out float dist))
                continue;

            Gizmos.DrawWireSphere(origin, a.hitboxes[i].radius);
            Gizmos.DrawWireSphere(secOrigin, a.hitboxes[i].radius);
            Vector3 r = Vector3.Cross(transform.up, a.hitboxes[i].orientation);
            Vector3 t = Vector3.Cross(r, a.hitboxes[i].orientation);
            Gizmos.DrawLine(origin + r.normalized * a.hitboxes[i].radius, secOrigin + r.normalized * a.hitboxes[i].radius);
            Gizmos.DrawLine(origin + -r.normalized * a.hitboxes[i].radius, secOrigin + -r.normalized * a.hitboxes[i].radius);
            Gizmos.DrawLine(origin + t.normalized * a.hitboxes[i].radius, secOrigin + t.normalized * a.hitboxes[i].radius);
            Gizmos.DrawLine(origin + -t.normalized * a.hitboxes[i].radius, secOrigin + -t.normalized * a.hitboxes[i].radius);
        }
    }
#endif
}
