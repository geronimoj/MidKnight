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
    /// The index of the attack to continue calling
    /// </summary>
    protected int attackIndex = 0;
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
            return cooldownTimer > 0;
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
    /// <summary>
    /// Calls the first 3 attacks from the phase attack by default
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public virtual void Attack(ref PlayerController c)
    {
        //If anything is null, return so we don't create errors
        if (Attacks == null)
        {
            Debug.LogError("The attack for Phase: " + this.name + " has not been assigned");
            return;
        }
        //Do we want to attack or are we already attacking
        if (Input.GetAxisRaw("Attack") != 0 || c.Attacking)
        {
            //If we are already attacking, continue the attack instead of starting a new one
            if (c.Attacking)
            {
                switch (attackIndex)
                {   //To avoid attack animations playing twice when landing or jumping during one
                    //The airborne animations have indexs 2,3,4. exactly + 3 of the original
                    case -1:
                    case 2:
                        //If we are on the ground, we cannot attack so undo all of this. This also applies to when we land
                        if (!c.animator.GetBool("Airborne"))
                        {
                            c.Attacking = false;
                            c.animator.SetBool("Attacking", false);
                            break;
                        }
                        Attacks.DownAttack(ref c);
                        break;
                    case 0:
                    case 3:
                        Attacks.DefaultAttack(ref c);
                        break;
                    case 1:
                    case 4:
                        Attacks.UpAttack(ref c);
                        break;
                    default:
                        //If none of the attacks passed for some reason, log an error
                        Debug.LogWarning("Player Attack Failed");
                        return;
                }
            }
            //This will only be entered when the attack is first called
            else
            {
                Debug.Log("Attack");
                float d = Input.GetAxisRaw("Vertical");
                //Set us to be attacking. This is set to false once the attack is complete automatically
                c.Attacking = true;
                //Check which attack we should do
                switch (d)
                {
                    case -1:
                        //If we are on the ground, we cannot attack so undo all of this
                        if (!c.animator.GetBool("Airborne"))
                        {
                            c.Attacking = false;
                            break;
                        }
                        Attacks.DownAttack(ref c);
                        attackIndex = -1;
                        break;
                    case 0:
                        Attacks.DefaultAttack(ref c);
                        attackIndex = 0;
                        break;
                    case 1:
                        Attacks.UpAttack(ref c);
                        attackIndex = 1;
                        break;
                    default:
                        //If none of the attacks passed for some reason, log an error
                        Debug.LogWarning("Player Attack Failed");
                        return;
                }
                if (c.animator.GetBool("Airborne"))
                    attackIndex += 3;
                c.animator.SetBool("Attacking", c.Attacking);
                c.animator.SetInteger("Attack", attackIndex);
            }
        }
    }

    public void DecrementCooldownTimer()
    {
        cooldownTimer -= Time.deltaTime;
    }
}
