using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirMove", menuName = "States/AirMove", order = 2)]
public class AirborneMove : State
{
    /// <summary>
    /// The maximum fall speed of the player
    /// </summary>
    public float maxFallSpeed = 0;
    /// <summary>
    /// How long the player can hold the jump key to extend the jump
    /// </summary>
    [Tooltip("How long the player can hold the jump key to not be affected by gravity")]
    public float jumpHoldTime = 0;
    /// <summary>
    /// How long the player hovers at the apex of the jump before falling if the jump key is released early
    /// </summary>
    [Tooltip("How long the player floats for after 'exiting' a jump early. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float floatTime = 0;
    /// <summary>
    /// How long it takes to decellerate to 0 vertical speed if the jump key is released early
    /// </summary>
    [Tooltip("How long it takes for the players vertical speed to reach 0. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float timeToZero = 0;
    /// <summary>
    /// The minimum fall speed of the player if they release the jump key early. The player quickly accelerates to this value
    /// </summary>
    [Tooltip("The fall speed the player will quickly accelerate too. ONLY APPLIES TO EARLY JUMP RELEASE")]
    [Range(0,100)]
    public float minFallSpeed = 0;
    /// <summary>
    /// The time it takes for the player to accelerate to the minimum fall speed
    /// </summary>
    [Tooltip("The time it takes to go from 0 velocity to min velocity vertically. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float timeToMinFallSpeed = 0;
    /// <summary>
    /// Set to true if the player is holding jump
    /// </summary>
    private bool holdingJump = false;
    /// <summary>
    /// Set to true if the player releases the jump key early
    /// </summary>
    private bool earlyJumpRelease = false;
    
    //TIMERS
    private float jumpTimer = 0;
    private float floatTimer = 0;
    private float decelToZeroTimer = 0;
    private float accelToMin = 0;

    public override void StateStart(ref PlayerController c)
    {   //Set the bools and timers
        earlyJumpRelease = false;
        holdingJump = false;
        jumpTimer = 0;
        floatTimer = 0;
        //If the player is holding the jump key, set the corresponding timers & values
        if (Input.GetAxisRaw("Jump") > 0)
        {
            jumpTimer = jumpHoldTime;
            floatTimer = floatTime;
            decelToZeroTimer = timeToZero;
            accelToMin = timeToMinFallSpeed;
            holdingJump = true;
        }
    }
    /// <summary>
    /// Moves the player with gravity
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public override void StateUpdate(ref PlayerController c)
    {   //Get the inputs
        float x = Input.GetAxisRaw("Horizontal");
        //If we are moving up, check if there is something directly above the player
        if (c.movement.VertSpeed > 0 && Physics.SphereCast(c.transform.position, c.PlayerRadius - 0.01f, Vector3.up, out RaycastHit hit, (c.Height / 2) + 0.01f, c.Ground))
        {   //Since its a sphere cast, we need to do an extra check to make sure the distance is just above the palyer
            if (hit.point.y - c.transform.position.y <= (c.Height / 2) + 0.01f)
            {   //Set us up to use the "short hop" decelleration function
                holdingJump = false;
                earlyJumpRelease = true;
                c.movement.VertSpeed = 0;
                accelToMin = timeToMinFallSpeed;
                floatTimer = 0;
            }
        }

        //Check if the player is still holding jump or if they are not allowed to hold it any longer
        if (holdingJump && (Input.GetAxisRaw("Jump") <= 0 || jumpTimer < 0))
            holdingJump = false;
        //Decrement the timer if they are still holding the button
        else if (holdingJump)
            jumpTimer -= Time.deltaTime;
        //Check if the player released the button early
        if (!holdingJump && jumpTimer > 0)
        {
            earlyJumpRelease = true;
            //Set jumpTimer to 0 so we don't call this block again
            jumpTimer = 0;
        }

        //Math for jumping with an early jump key release
        if (earlyJumpRelease)
        {
            //Decellerate the player to 0
            if (c.movement.VertSpeed > 0)
            {   //Slow down the players veritcal speed and the timer
                c.movement.VertSpeed -= (c.movement.VertSpeed / decelToZeroTimer) * Time.deltaTime;
                decelToZeroTimer -= Time.deltaTime;
                //If the timer has reached 0, set the players speed to 0 so we don't overshoot or undershoot the mark
                if (decelToZeroTimer <= 0)
                    c.movement.VertSpeed = 0;
            }
            //Float for floatTime
            else if (c.movement.VertSpeed == 0)
                floatTimer -= Time.deltaTime;
            //Accelerate to min speed
            if (floatTimer <= 0 && c.movement.VertSpeed <= 0)
            {   //Accelerate the player downwards and increment the timer
                c.movement.VertSpeed -= ((minFallSpeed - c.movement.VertSpeed) / accelToMin) * Time.deltaTime;
                accelToMin -= Time.deltaTime;
                //If the timer has complete, set the players vertical speed to minFallSpeed so we don't over or under shoot
                if (accelToMin <= 0)
                    c.movement.VertSpeed = minFallSpeed;
            }

            //If we have reached the speed, so we can go back to normal gravity simulation
            if (c.movement.VertSpeed < -minFallSpeed)
                earlyJumpRelease = false;
        }

        //Find the players speed
        float speed = 0;
        if (x != 0)
            speed = c.MoveSpeed;
        //Get the direction to the right
        Vector3 right = c.gm.GetPathDirectionRight(c.transform.position);
        //Check if we are facing right
        if (x > 0)
            c.FacingRight = true;
        //Invert the vector if we are trying to go left
        if (x < 0)
        {   //Update because we are facing left
            c.FacingRight = false;
            right = -right;
        }
        //Set the horizontal direction of the player to that direction
        right *= speed;

        c.movement.Direction = right;
        //Set the players speed
        c.movement.HozSpeed = speed;

        if (!holdingJump && !earlyJumpRelease)
            c.movement.VertSpeed -= c.Gravity * Time.deltaTime;
        //Cap the players fall speed if necessary
        if (maxFallSpeed > 0)
            c.movement.VertSpeed = Mathf.Clamp(c.movement.VertSpeed, -maxFallSpeed, Mathf.Infinity);

        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
    }
}
