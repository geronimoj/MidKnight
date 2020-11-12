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
    /// The index of the phase to swap too
    /// </summary>
    [SerializeField]
    private int swapToIndex = 0;
    /// <summary>
    /// An array containing every moon phase, unlocked or not
    /// </summary>
    public MoonPhase[] everyMoonPhase;
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

    private bool newCycleInput = false;

    private bool newSwapInput = false;

    /// <summary>
    /// Calls update on the current phase if there is one
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void PhaseUpdate(PlayerController c)
    {   //Timer for the OnDrawGizmos
#if UNITY_EDITOR
        //Increment the timer
        timer += Time.deltaTime;
#endif
        CyclePhase(ref c);
        cooldownTimer -= Time.deltaTime;
        DecrementTimers();
        if (current != null)
        {
            current.DoPhase(ref c);
#if UNITY_EDITOR
            bool newAttack = c.Attacking;
#endif
            //If we can attack, check the attacks
            if (c.CanAttack)
            {   //Perform the attack if we can
                current.Attack(ref c);
#if UNITY_EDITOR
                //In the editor we draw the hitboxes only when the player attacks
                if (newAttack != c.Attacking)
                {
                    displayAttack = c.Attacking;
                    timer = 0;
                    attackIndexToDisplay = c.animator.GetInteger("Attack");
                    if (attackIndexToDisplay > 1)
                        attackIndexToDisplay -= 3;

                    switch (attackIndexToDisplay)
                    {
                        case -1:
                            attackIndexToDisplay = 2;
                            break;
                        case 0:
                            attackIndexToDisplay = 0;
                            break;
                        case 1:
                            attackIndexToDisplay = 1;
                            break;
                    }
                }
#endif
            }
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
    public void SwapPhase(MoonPhase target, ref PlayerController c)
    {   //Make sure the target is valid vand we can swap to it
        if (cooldownTimer > 0 || target == null || target.OnCooldown)
            return;
        //Set the cooldown timer
        cooldownTimer = swapCooldown;
        c.GainBonusDamage();
        //Exit
        current.PhaseExit(ref c);
        current.OnExit.Invoke();
        //We do this outside of the OnExit so that if someone overrides it, it doesn't poop itself & so we can call OnExit
        //when exiting the eclipse mode to "deactivate" the phase without putting it on cooldown
        current.PutOnCooldown();
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
    /// Or if the current phase is Eclipse.
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
    /// <summary>
    /// Checks if the player wants to cycle between moon phases & swaps them if they request to.
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    private void CyclePhase(ref PlayerController c)
    {
        float cycleDir = Input.GetAxis("CyclePhase");

        if (cycleDir > 0 && newCycleInput)
        {
            swapToIndex = SetSwapIndex(swapToIndex, true, ref c);
            newCycleInput = false;
        }
        else if (cycleDir < 0 && newCycleInput)
        {
            swapToIndex = SetSwapIndex(swapToIndex, false, ref c);
            newCycleInput = false;
        }
        else if (cycleDir == 0)
            newCycleInput = true;

        if (swapToIndex < 0)
        {
            Debug.LogError("Could not find valid, unlocked moon phase to cycle to. Setting cycle index to 0");
            swapToIndex = 0;
            return;
        }

        cycleDir = Input.GetAxis("SelectPhase");
        //If we press R, swap to the phase at swapToIndex
        if (cooldownTimer < 0 && cycleDir > 0 && newSwapInput)
        {
            SwapPhase(everyMoonPhase[swapToIndex], ref c);
            newSwapInput = false;
        }
        else if (cycleDir == 0)
            newSwapInput = true;
        
    }
    /// <summary>
    /// Takes the expected index of the phase to swap to and checks if it is a valid index & the phase is unlocked
    /// </summary>
    /// <param name="expectedIndex">The index to start from</param>
    /// <returns>Returns the new index to swap too</returns>
    public int SetSwapIndex(int expectedIndex, bool indexUpwards, ref PlayerController c)
    {
        int initial = expectedIndex;

        for (int i = expectedIndex; i < everyMoonPhase.Length;)
        {
            //Decrement or increment expectedindex
            if (indexUpwards)
            {
                expectedIndex++;
                //Clamp i & expectedindex between 0 & everyMoonPhase.Length
                if (expectedIndex >= everyMoonPhase.Length)
                    expectedIndex = 0;
            }
            else
            {
                expectedIndex--;
                //Clamp i & expectedindex between 0 & everyMoonPhase.Length
                if (expectedIndex < 0)
                    expectedIndex = everyMoonPhase.Length - 1;
            }
            //Check if this index is an unlocked phase.
            if (c.ut.GetKeyValue(everyMoonPhase[expectedIndex].phaseID) && everyMoonPhase[expectedIndex] != current)
                //If it is, return the index
                return expectedIndex;
            
            //Check if expectedindex = initial. If so, return -1
            //This means we've looped through all the indexes & none of them are unlocked
            if (expectedIndex == initial)
                break;
            i = expectedIndex;
        }
        //Return -1 because we failed to find a valid moonPhase
        return -1;
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
