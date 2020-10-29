using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(UnlockTracker))]
public class PlayerController : Character
{
    private StateManager manager;

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
    /// Gets a reference to the State & Game Managers
    /// </summary>
    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
        ut = GetComponent<UnlockTracker>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        if (gm == null)
            Debug.LogError("GameManager not found. Check GameObject tagged GameManager has GameManager");
        if (animator == null)
            Debug.LogError("Animator not found on Child at index 0 on Player");
        if (manager == null)
            Debug.LogError("StateManager not found on GameObject Player. Assign manager to make player work");
        if (ut == null)
            Debug.LogError("UnlockTracker not found on Player");
    }
    /// <summary>
    /// Calls start on the current state
    /// </summary>
    private void Start()
    {
        manager.CallStart(this);
        cc.stepOffset = 0;
    }
    /// <summary>
    /// Decrements the timer and calls update on the state
    /// </summary>
    private void Update()
    {
        dashTimer -= Time.deltaTime;
        manager.DoState(this);
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
    public void DoDash()
    {
        dashTimer = dashCooldown;
    }
}
