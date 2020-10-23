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

    [SerializeField]
    private float gravity = 1f;

    [SerializeField]
    private float onJumpForce = 1f;

    [SerializeField]
    private float dashCooldown = 0;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public float Gravity
    {
        get
        {
            return gravity;
        }
    }

    public float Height
    {
        get
        {
            return cc.height;
        }
    }

    public float PlayerRadius
    {
        get
        {
            return cc.radius;
        }
    }

    public float OnJumpForce
    {
        get
        {
            return onJumpForce;
        }
    }

    public bool CanDash
    {
        get
        {
            return dashTimer <= 0;
        }
    }

    private bool facingRight = false;

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

    private float dashTimer = 0;

    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        manager.CallStart(this);
    }

    private void Update()
    {
        dashTimer -= Time.deltaTime;
        manager.DoState(this);
    }

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

        cc.Move(moveVec);

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
