using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(UnlockTracker))]
[RequireComponent(typeof(PhaseManager))]
public class PlayerController : Character
{
    private static PlayerController player;

    public static PlayerController Player
    {
        get
        {
            return player;
        }
    }
    //Its up here so it looks good in the inspector
    /// <summary>
    /// The angle of the knockback
    /// </summary>
    [SerializeField]
    [Range(0, 90)]
    protected float knockBackAngle = 20f;
    /// <summary>
    /// Reference to the state manager
    /// </summary>
    private StateManager manager;
    /// <summary>
    /// Reference to the Phase Manager
    /// </summary>
    private PhaseManager phase;
    /// <summary>
    /// Reference to the unlock tracker
    /// </summary>
    [HideInInspector]
    public UnlockTracker ut;
    /// <summary>
    /// Reference to the gameManager
    /// </summary>
    [HideInInspector]
    public GameManager gm;
    /// <summary>
    /// Reference to the animator
    /// </summary>
    [HideInInspector]
    public Animator animator;

    /// <summary>
    /// The layermask that we can stand on
    /// </summary>
    public LayerMask ground;
    /// <summary>
    /// The storage location for the players movement infromation
    /// </summary>
    public Movement movement;
    /// <summary>
    /// The speed of the player
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1f;
    /// <summary>
    /// The downwards acceleration of the player when airborne
    /// </summary>
    [SerializeField]
    private float gravity = 1f;
    /// <summary>
    /// The upwards speed of the player when they jump
    /// </summary>
    [SerializeField]
    private float onJumpForce = 1f;
    /// <summary>
    /// The cooldown of the dash
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float dashCooldown = 0;
    /// <summary>
    /// How long the players i-Frames last after taking damage
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    private float iFrames = 0;
    /// <summary>
    /// A timer for the players iframes
    /// </summary>
    private float iFrameTimer = 0;
    /// <summary>
    /// The maximum moonLight the player can have
    /// </summary>
    [SerializeField]
    [Range(0, 1000)]
    private float moonLightCap = 100;
    /// <summary>
    /// The current moonLight the player has
    /// </summary>
    /// So we can see it in inspector
    [SerializeField]
    private float moonLight = 0;
    /// <summary>
    /// How long the bonusDamage lasts
    /// </summary>
    [SerializeField]
    [Tooltip("The time the player must swap phases to keep bonus damage")]
    [Range(0, 100)]
    private float bonusDamageLifeTime = 1;
    /// <summary>
    /// The timer for bonusDamageLifeTime
    /// </summary>
    private float bonusDamageTimer = 0;
    /// <summary>
    /// Set to true if the player took damage this frame.
    /// Necessary for breaking out of healing & dash on damage
    /// </summary>
    private bool tookDamageThisLoop = false;

    /// <summary>
    /// Is true when the player dies
    /// </summary>
    private bool dead = false;
    /// <summary>
    /// Returns true if the player is dead
    /// </summary>
    public bool Dead
    {
        get
        {
            return dead;
        }
    }
    /// <summary>
    /// A Get for moveSpeed
    /// </summary>
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }
    /// <summary>
    /// A Get for gravity
    /// </summary>
    public float Gravity
    {
        get
        {
            return gravity;
        }
    }
    /// <summary>
    /// A Get for the players height
    /// </summary>
    public float Height
    {
        get
        {
            return cc.height;
        }
    }
    /// <summary>
    /// A Get for the players radius
    /// </summary>
    public float PlayerRadius
    {
        get
        {
            return cc.radius;
        }
    }
    /// <summary>
    /// A Get for onJumpForce
    /// </summary>
    public float OnJumpForce
    {
        get
        {
            return onJumpForce;
        }
    }
    /// <summary>
    /// The timer for if the player can dash
    /// </summary>
    private float dashTimer = 0;
    /// <summary>
    /// Is true if we are allowed to dash
    /// </summary>
    private bool canDash = false;
    /// <summary>
    /// Returns true if the player can dash
    /// Sets if the player is allowed to dash. Wether the player can dash is still determined by an external timer tho
    /// </summary>
    public bool CanDash
    {
        get
        {
            return dashTimer <= 0 && canDash;
        }
        set
        {
            canDash = value;
        }
    }
    /// <summary>
    /// Whether the player is allowed to jump again while airborne
    /// </summary>
    private bool canJumpAgain = false;
    /// <summary>
    /// Returns true if the player can double jump or is on the ground
    /// </summary>
    public bool CanJump
    {
        get
        {
            if (animator.GetBool("Airborne") && canJumpAgain && ut.GetKeyValue("double jump"))
            {
                canJumpAgain = false;
                return true;
            }
            else if (!animator.GetBool("Airborne"))
                return true;

            return false;
        }
    }
    /// <summary>
    /// A storage location for the direction the player is facing relative to the path
    /// </summary>
    private bool facingRight = false;
    /// <summary>
    /// A Get/Set for facingRight
    /// </summary>
    public bool FacingRight
    {
        get
        {
            return facingRight;
        }
        set
        {
            facingRight = value;
        }
    }
    /// <summary>
    /// A Get for ground
    /// </summary>
    public LayerMask Ground
    {
        get
        {
            return ground;
        }
    }
    /// <summary>
    /// Returns true when the players iFrames have dropped below 0
    /// </summary>
    public bool CanTakeDamage
    {
        get
        {
            return iFrameTimer <= 0;
        }
    }
    /// <summary>
    /// The cap for bonus damage
    /// </summary>
    [Range(0, 100)]
    [SerializeField]
    private int bonusDamageCap = 0;
    /// <summary>
    /// A get for the bonus damage cap
    /// </summary>
    public int BonusDamageCap
    {
        get
        {
            return bonusDamageCap;
        }
    }

    /// <summary>
    /// The damage of the attacks
    /// </summary>
    [Tooltip("The damage dealt on hit")]
    [SerializeField]
    private int damage = 0;
    /// <summary>
    /// A Get for damage. does not include bonus damage
    /// </summary>
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    /// <summary>
    /// The knockBack of any given attack
    /// </summary>
    [Tooltip("The knockback dealt on hit")]
    [Range(0, 100)]
    [SerializeField]
    private float knockBack = 0;
    /// <summary>
    /// A Get for the base knockback. This will never change
    /// </summary>
    public float BaseKnockBack
    {
        get
        {
            return knockBack;
        }
    }
    /// <summary>
    /// Where we store the actual knockback value. This one gets changed
    /// </summary>
    private float actualKnockback = 0;
    /// <summary>
    /// The current knockback value
    /// </summary>
    public float Knockback
    {
        get
        {
            return actualKnockback;
        }
        set
        {
            actualKnockback = value;
        }
    }
    /// <summary>
    /// The upward force to apply to the player when they hit an enemy from above while airborne
    /// </summary>
    [Tooltip("The upwards speed given to the player when hitting an enemy with the down attack while airborne")]
    [Range(0, 100)]
    [SerializeField]
    private float pogoForce = 0;
    /// <summary>
    /// A Get for pogoForce
    /// </summary>
    public float PogoForce
    {
        get
        {
            return pogoForce;
        }
    }
    /// <summary>
    /// How much moonLight will be gained on hit
    /// </summary>
    [Tooltip("The moonlight gain on hit")]
    [Range(0, 1000)]
    [SerializeField]
    private float moonLightGain = 0;
    /// <summary>
    /// A Get for the moonlight to gain per hit
    /// </summary>
    public float MoonLightGain
    {
        get
        {
            return moonLightGain;
        }
    }

    /// <summary>
    /// How much bonus damage the player has to their attacks
    /// </summary>
    private int bonusDamage = 0;
    /// <summary>
    /// A Get/Set for bonusDamage
    /// </summary>
    public int BonusDamage
    {
        get
        {
            return bonusDamage;
        }
        set
        {
            bonusDamage = value;
        }
    }
    /// <summary>
    /// Gets or Sets the moonLight of the player. Caps the moonlight between 0 & moonLight cap
    /// </summary>
    public float MoonLight
    {
        get
        {
            return moonLight;
        }
        set
        {
            moonLight = value;
            moonLight = Mathf.Clamp(moonLight, 0, moonLightCap);
        }
    }
    /// <summary>
    /// Set to true when the player is attacking
    /// </summary>
    private bool attacking = false;
    /// <summary>
    /// A Get/Set for if the player is attacking
    /// </summary>
    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }
    /// <summary>
    /// Set to true when the player can attack
    /// </summary>
    private bool canAttack = true;
    /// <summary>
    /// A Get/Set for can attack
    /// </summary>
    public bool CanAttack
    {
        get
        {
            return canAttack;
        }
        set
        {
            canAttack = value;
        }
    }
    /// <summary>
    /// The cooldown duration of moonBeam
    /// </summary>
    [Range(0,100)]
    public float moonBeamCooldown = 0;
    /// <summary>
    /// A timer for moonBeam's cooldown
    /// </summary>
    private float moonBeamTimer = 0;
    /// <summary>
    /// Returns true if the moonBeam can be cast
    /// </summary>
    public bool CanCastMoonBeam
    {
        get
        {
            return moonBeamTimer < 0;
        }
    }
    /// <summary>
    /// Gets a reference to the State & Game Managers
    /// </summary>
    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
        phase = GetComponent<PhaseManager>();
        ut = GetComponent<UnlockTracker>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        if (gm == null)
        {
            Debug.LogError("GameManager not found. Check GameObject tagged GameManager has GameManager");
            Debug.Break();
        }
        if (animator == null)
        {
            Debug.LogError("Animator not found on Child at index 0 on Player");
            Debug.Break();
        }
        if (manager == null)
            Debug.LogError("StateManager not found on GameObject Player. Assign manager to make player work");
        if (ut == null)
            Debug.LogError("UnlockTracker not found on Player");
        if (phase == null)
            Debug.LogError("PhaseManager not found on Player GameObject");
    }
    /// <summary>
    /// Calls start on the current state
    /// </summary>
    private void Start()
    {
        SetHealth = MaxHealth;
        manager.CallStart(this);
        phase.PhaseStart(this);
        Attacking = false;
        cc.stepOffset = 0;
        actualKnockback = knockBack;
        dead = false;
        player = this;
    }
    /// <summary>
    /// Decrements the timer and calls update on the state
    /// </summary>
    protected override void ExtraUpdate()
    {   //Is the player dead
        if (dead)
            return;
        //Decrement the timers
        if (!CanTakeDamage)
            iFrameTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
        bonusDamageTimer -= Time.deltaTime;
        moonBeamTimer -= Time.deltaTime;
        
        //If the timer for bonus damage is finished, set bonus damage to 0
        if (bonusDamageTimer < 0)
            bonusDamage = 0;

        manager.DoState(this);
        phase.PhaseUpdate(this);
        //Get the players direction just to save excess cpu
        Vector3 dir = movement.Direction;
        //Rotate to look along the direction. We have to rotate the direction by 90 degrees to the "left", since we move along our x axis
        //And LookRotation wants the forward to be the z axis. This points dir either into our away from the screen, correctly rotating us
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(-dir.z, dir.y, dir.x), Vector3.up);
        //If we took damage earlier, reset took damage
        tookDamageThisLoop = false;
    }
    /// <summary>
    /// Moves the player along the vector given
    /// </summary>
    /// <param name="moveVec">The direction of movement</param>
    public override void Move(Vector3 moveVec)
    {   //If we are in hit stun, don't move
        if (hitstunTimer > 0)
            return;
        if (knockBackTimer > 0)
        {
            moveVec = knockBackDir * knockBackForce;
            //Calculate the change in the vectors
            Vector3 v = movement.MoveVec - moveVec;
            //Get the change as a percentage of how much knockback we have left to enforce.
            v *= 1 - (knockBackTimer / knockBackDuration);
            //Apply the change to the x & z component by not the y as we want that to be affected by gravity
            moveVec.x += v.x;
            moveVec.z += v.z;
            //Apply gravity. We can figure out how much to add thanks to the knockback timer
            moveVec.y -= Gravity * (knockBackDuration - knockBackTimer);
            //Apply time
            moveVec *= Time.deltaTime;
        }
        if (gm == null)
        {   //If we don't have a gameManager, move along the x axis only
            moveVec.z = 0;
            cc.Move(moveVec);
            return;
        }
        //Update the moveVector to be along the path
        moveVec = gm.MoveAlongPath(transform.position, moveVec);
        //Do a raycast to check for ramps or ramped objects
        if (Physics.Raycast(transform.position + moveVec, Vector3.down, out RaycastHit hit, (Height / 2) - 0.01f, Ground))
            moveVec.y = (hit.point.y + (Height / 2)) - transform.position.y;
        //Move the player
        cc.Move(moveVec);
        Vector3 pos = transform.position + Vector3.down * (cc.height / 2 - cc.radius + cc.radius / 3);
        Debug.DrawLine(pos, pos + transform.right);
        if (Physics.Raycast(pos, transform.right, out hit, cc.radius, ground))
        {
            float f = Vector3.Distance(pos, hit.point);
            cc.Move(-transform.right * (cc.radius - f));
        }
        else if (Physics.Raycast(pos, -transform.right, out hit, cc.radius, ground))
        {
            float f = Vector3.Distance(pos, hit.point);
            cc.Move(transform.right * (cc.radius - f));
        }
        //Ensure the player has not escaped the map just in case moving did such a thing
        //Disable the character controller to allow for teleportation. It has an internal
        //sence of motion and stuffs up teleportation
        cc.enabled = false;
        transform.position = gm.SnapToPath(transform.position);
        //Re-enable the cc
        cc.enabled = true;
    }

    /// <summary>
    /// Called when the dash is performed
    /// </summary>
    public void OnDash()
    {
        dashTimer = dashCooldown;
        CanDash = false;
    }
    /// <summary>
    /// Called when the player jumps
    /// </summary>
    public void OnJump()
    {
        if (!canJumpAgain && animator.GetBool("Airborne"))
             manager.CallStart(this);
    }
    /// <summary>
    /// Called when the player lands
    /// </summary>
    public void OnLand()
    {
        canJumpAgain = true;
        CanDash = true;
    }
    /// <summary>
    /// Called when moonBeam is casted
    /// </summary>
    public void DoMoonBeam()
    {
        moonBeamTimer = moonBeamCooldown;
    }
    /// <summary>
    /// Deals damage to the player with iframes included
    /// </summary>
    /// <param name="damage">How much damage to deal</param>
    public override void TakeDamage(int damage)
    {   //If the damage is negative, its healing.
        if (damage < 0)
        {   //Heal
            SetHealth = Health - damage;
            return;
        }
        //Can the player take damage
        if (CanTakeDamage)
        {   //Set the iFrame timer
            iFrameTimer = iFrames;
            hitstunTimer = hitstunDuration;
            knockBackTimer = knockBackDuration;
            //Deal damage
            SetHealth = Health - damage;
            //Set us to have taken damage this frame
            tookDamageThisLoop = true;
            //Trigger the damage animation
            animator.SetTrigger("TookDamage");
            //Log that damage was dealt
#if UNITY_EDITOR
            Debug.Log("Took Damage");
#endif

            if (Health <= 0)
                OnDeath();
        }
    }
    /// <summary>
    /// Sets the i frame timer to duration
    /// </summary>
    /// <param name="duration">The length of the i frames</param>
    public void SetIFrames(float duration)
    {
        iFrameTimer = duration;
    }
    /// <summary>
    /// Calls the phasemanagers CorrectPhase function & returns the results
    /// </summary>
    /// <param name="ID">The ID of the phase we want to check if we are in</param>
    /// <returns>Returns true if the ID of the current phase is equal to ID</returns>
    public bool CurrentPhaseIDCompare(string ID)
    {
        return phase.CorrectPhase(ID);
    }
    /// <summary>
    /// Gives the player 1 unit of bonus damage and sets the timer
    /// </summary>
    public void GainBonusDamage()
    {   //Increase the damage
        bonusDamage++;
        //Clamp the damage bonus
        if (bonusDamage > BonusDamageCap)
            bonusDamage = bonusDamageCap;
        //Reset the damage bonus liftime timer
        bonusDamageTimer = bonusDamageLifeTime;
    }
    /// <summary>
    /// Returns true if the player took damage this frame.
    /// Set to false at the end of their update cycle
    /// </summary>
    /// <returns>Returns true if the player took damage this frame</returns>
    public bool TookDamageThisFrame()
    {
        return tookDamageThisLoop;
    }

    public override void OnDeath()
    {
        animator.SetTrigger("Dead");
        Debug.Log("Player is dead");
        dead = true;
    }

    public override void SetKnockBackDirection(Vector3 dir)
    {
        knockBackDir = dir.normalized;
        //Check if the knockback direction is directly up
        //This is to cover edge cases where knockback dir was directly upwards
        if (Vector3.Dot(dir, Vector3.up) > 0.95)
            knockBackDir = transform.right;
        //Force the y value to be the knockback angle
        knockBackDir.y = Mathf.Sin(knockBackAngle * Mathf.Deg2Rad);

    }
}
