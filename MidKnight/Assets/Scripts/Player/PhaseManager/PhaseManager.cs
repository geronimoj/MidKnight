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
    /// <summary>
    /// A list of preivously active phases
    /// </summary>
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
        //Increment the timer
        timer += Time.deltaTime;
        //If we have gone pass the duration, reset the timer
        if (timer > current.Attacks.attacks[attackIndexToDisplay].duration)
            timer = 0;
#endif
        cooldownTimer -= Time.deltaTime;
        DecrementTimers();
        if (current != null)
        {
            current.DoPhase(ref c);
            current.Attack(ref c);
        }
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
    {   //Make sure the target is valid vand we can swap to it
        if (cooldownTimer > 0 || target == null || target.OnCooldown)
            return;
        //Set the cooldown timer
        cooldownTimer = swapCooldown;
        c.GainBonusDamage();
        //Exit
        current.PhaseExit(ref c);
        current.OnExit.Invoke();
        //Swap
        current = target;
        OnSwap.Invoke();
        //Enter
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
    /// <summary>
    /// Called to decrement to cooldown timers of phases that have been equiped
    /// </summary>
    private void DecrementTimers()
    {
        for (int i = 0; i < knownPhases.Count; i++)
            knownPhases[i].DecrementCooldownTimer();
    }
#if UNITY_EDITOR
    private float timer = 0;
    [Range(0,2)]
    public int attackIndexToDisplay;
    public bool displayAttack;
    /// <summary>
    /// Draws the PhaseAttack hitboxes
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!displayAttack)
            return;
        //Make sure the index is valid
        if (current == null || current.Attacks == null || current.Attacks.attacks.Length < attackIndexToDisplay + 1 || current.Attacks.attacks[attackIndexToDisplay] == null)
            return;
        //Store a reference to it to make code look cleaner
        Attack a = current.Attacks.attacks[attackIndexToDisplay];
        //GREEN
        Gizmos.color = Color.green;
        //Loop through the hitboxes of the attack
        for (int i = 0; i < a.hitboxes.Length; i++)
        {   //Check if the hitbox is active and, if so, where it is
            if (!a.GetHitBoxInfo(a.hitboxes[i], transform, timer, Time.deltaTime, out Vector3 origin, out Vector3 secOrigin, out Vector3 dir, out float dist))
                continue;
            //Draw the front and end of the capsual hitbox
            Gizmos.DrawWireSphere(origin, a.hitboxes[i].radius);
            Gizmos.DrawWireSphere(secOrigin, a.hitboxes[i].radius);
            //Calculate the vectors "above" and "to the right" of the hitboxes orientation
            Vector3 r = Vector3.Cross(transform.up, a.hitboxes[i].orientation);
            Vector3 t = Vector3.Cross(r, a.hitboxes[i].orientation);
            //Draw the lines along the side of the spheres to create the appearance of a capsual
            //Left & Right
            Gizmos.DrawLine(origin + r.normalized * a.hitboxes[i].radius, secOrigin + r.normalized * a.hitboxes[i].radius);
            Gizmos.DrawLine(origin + -r.normalized * a.hitboxes[i].radius, secOrigin + -r.normalized * a.hitboxes[i].radius);
            //Top & bottom
            Gizmos.DrawLine(origin + t.normalized * a.hitboxes[i].radius, secOrigin + t.normalized * a.hitboxes[i].radius);
            Gizmos.DrawLine(origin + -t.normalized * a.hitboxes[i].radius, secOrigin + -t.normalized * a.hitboxes[i].radius);
        }
    }
#endif
}
