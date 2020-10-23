using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
public class PlayerController : Character
{
    private StateManager manager;

    public GameManager gm;
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
    /// Gets a reference to the State & Game Managers
    /// </summary>
    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    /// <summary>
    /// Calls start on the current state
    /// </summary>
    private void Start()
    {
        manager.CallStart(this);
    }
    /// <summary>
    /// Decrements the timer and calls update on the state
    /// </summary>
    private void Update()
    {
        dashTimer -= Time.deltaTime;
        manager.DoState(this);
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
        //Find our distance along the path

        //Move moveVec along the path and store its final position

        //Set moveVec to be the difference between our current position and the final position

        //Check if there are slopes or walls in the way of moveVec and adjust moveVec accordingly

        //Move the player
        cc.Move(moveVec);
        //Ensure the player has not escaped the map
        transform.position = gm.SnapToPath(transform.position);
    }

    /// <summary>
    /// Called when the dash is performed
    /// </summary>
    public void DoDash()
    {
        dashTimer = dashCooldown;
    }
}
