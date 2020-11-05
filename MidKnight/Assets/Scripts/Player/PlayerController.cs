using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(UnlockTracker))]
[RequireComponent(typeof(PhaseManager))]
public class PlayerController : Character
{
    private StateManager manager;

    private PhaseManager phase;

    [HideInInspector]
    public UnlockTracker ut;

    [HideInInspector]
    public GameManager gm;

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
    /// Returns true if the player can dash
    /// </summary>
    public bool CanDash
    {
        get
        {
            return dashTimer <= 0;
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
    /// How much bonus damage the player has to their attacks
    /// </summary>
    [SerializeField]
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
    /// Set to true when the player is attacking
    /// </summary>
    private bool attacking = false;
    /// <summary>
    /// Used internally to determine which attack needs to be called in the update loop if an attack has begun
    /// </summary>
    private int attackIndex = 0;
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
        manager.CallStart(this);
        phase.PhaseStart(this);
        Attacking = false;
        cc.stepOffset = 0;
    }
    /// <summary>
    /// Decrements the timer and calls update on the state
    /// </summary>
    private void Update()
    {
        if (!CanTakeDamage)
            iFrameTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
        manager.DoState(this);
        phase.PhaseUpdate(this);
        Attack();
        //Get the players direction just to save excess cpu
        Vector3 dir = movement.Direction;
        //Rotate to look along the direction. We have to rotate the direction by 90 degrees to the "left", since we move along our x axis
        //And LookRotation wants the forward to be the z axis. This points dir either into our away from the screen, correctly rotating us
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(-dir.z, dir.y, dir.x), Vector3.up);
    }
    /// <summary>
    /// Moves the player along the vector given
    /// </summary>
    /// <param name="moveVec">The direction of movement</param>
    public override void Move(Vector3 moveVec)
    {
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
    public void DidDash()
    {
        dashTimer = dashCooldown;
    }
    /// <summary>
    /// Called when the player jumps
    /// </summary>
    public void DidJump()
    {
        if (!canJumpAgain && animator.GetBool("Airborne"))
             manager.CallStart(this);
    }
    /// <summary>
    /// Called when the player lands
    /// </summary>
    public void DidLand()
    {
        canJumpAgain = true;
    }
    /// <summary>
    /// Deals damage to the player with iframes included
    /// </summary>
    /// <param name="damage">How much damage to deal</param>
    public override void TakeDamage(int damage)
    {   //Can the player take damage
        if (CanTakeDamage)
        {   //Set the iFrame timer
            iFrameTimer = iFrames;
            //Deal damage
            SetHealth = Health - damage;
            //Log that damage was dealt
#if UNITY_EDITOR
            Debug.Log("Took Damage");
#endif
        }
    }
    /// <summary>
    /// Checks and calls which attack the player should perform
    /// </summary>
    private void Attack()
    {   //Do we want to attack
        if (Input.GetAxisRaw("Attack") != 0 || Attacking)
        {
            //If anything is null, return so we don't create errors
            if (phase == null || phase.CurrentPhase == null || phase.CurrentPhase.Attacks == null)
            {
                Debug.LogError("The phase manager, current phase or phase attack for the current phase has not been assigned");
                return;
            }
            //If we are already attacking, continue the attack instead of starting a new one
            if (Attacking)
            {
                switch (attackIndex)
                {   //To avoid attack animations playing twice when landing or jumping during one
                    //The airborne animations have indexs 2,3,4. exactly + 3 of the original
                    case -1:
                    case 2:
                        //If we are on the ground, we cannot attack so undo all of this. This also applies to when we land
                        if (!animator.GetBool("Airborne"))
                        {
                            Attacking = false;
                            animator.SetBool("Attacking", false);
                            break;
                        }
                        phase.CurrentPhase.Attacks.DownAttack(this);
                        break;
                    case 0:
                    case 3:
                        phase.CurrentPhase.Attacks.DefaultAttack(this);
                        break;
                    case 1:
                    case 4:
                        phase.CurrentPhase.Attacks.UpAttack(this);
                        break;
                    default:
                        //If none of the attacks passed for some reason, log an error
                        Debug.LogWarning("Player Attack Failed");
                        return;
                }
                return;
            }
            //This will only be entered when the attack is first called
            else
            {
                float d = Input.GetAxisRaw("Vertical");
                //Set us to be attacking. This is set to false once the attack is complete automatically
                Attacking = true;
                //Check which attack we should do
                switch (d)
                {
                    case -1:
                        //If we are on the ground, we cannot attack so undo all of this
                        if (!animator.GetBool("Airborne"))
                        {
                            Attacking = false;
                            break;
                        }
                        phase.CurrentPhase.Attacks.DownAttack(this);
                        attackIndex = -1;
                        break;
                    case 0:
                        phase.CurrentPhase.Attacks.DefaultAttack(this);
                        attackIndex = 0;
                        break;
                    case 1:
                        phase.CurrentPhase.Attacks.UpAttack(this);
                        attackIndex = 1;
                        break;
                    default:
                        //If none of the attacks passed for some reason, log an error
                        Debug.LogWarning("Player Attack Failed");
                        return;
                }
                if (animator.GetBool("Airborne"))
                    attackIndex += 3;
                animator.SetBool("Attacking", Attacking);
                animator.SetInteger("Attack", attackIndex);
            }
        }
    }
}
