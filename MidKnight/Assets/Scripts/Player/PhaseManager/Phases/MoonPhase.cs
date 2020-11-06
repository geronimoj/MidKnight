using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoonPhase", menuName = "MoonPhases/Empty", order = 0)]
public class MoonPhase : ScriptableObject
{   
    /// <summary>
    /// The ID of this phase
    /// </summary>
    public string phaseID = "NULL";
    /// <summary>
    /// How long this phase should go on cooldown for when exiting it
    /// </summary>
    public float phaseCooldown = 0;
    /// <summary>
    /// The timer for the phases cooldown
    /// </summary>
    private float cooldownTimer = 0;
    /// <summary>
    /// The attacks to use for this phase
    /// </summary>
    public PhaseAttack attack;
    /// <summary>
    /// Set to true if this is the active phase.
    /// </summary>
    private bool active = false;
    /// <summary>
    /// Returns true if this is the active phase
    /// </summary>
    protected bool Active
    {
        get
        {
            return active;
        }
    }
    /// <summary>
    /// A Get for the phase attacks so the attack functions can be called
    /// </summary>
    public PhaseAttack Attacks
    {
        get
        {
            return attack;
        }
    }
    /// <summary>
    /// Returns true if the phase is on cooldown
    /// </summary>
    public bool OnCooldown
    {
        get
        {
            return cooldownTimer <= 0;
        }
    }
    /// <summary>
    /// A reference to the player controller for any functions that need to be hooked up to unity events
    /// </summary>
    protected PlayerController pc;
    /// <summary>
    /// Called when entering the phase
    /// </summary>
    public UnityEvent OnEnter;
    /// <summary>
    /// Called when exiting the phase
    /// </summary>
    public UnityEvent OnExit;
    /// <summary>
    /// Called when entering the phase
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void PhaseEnter(ref PlayerController c)
    {
        pc = c;
        active = true;
    }
    /// <summary>
    /// Called when in the phase. Also calls PhaseUpdate
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void DoPhase(ref PlayerController c)
    {
        PhaseUpdate(ref c);
    }
    /// <summary>
    /// A virtual function for adding to DoPhase
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void PhaseUpdate(ref PlayerController c) { }
    /// <summary>
    /// Called when the phase exits
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void PhaseExit(ref PlayerController c) 
    {
        cooldownTimer = phaseCooldown;
        active = false;
    }

    public void DecrementCooldownTimer()
    {
        cooldownTimer -= Time.deltaTime;
    }
}
