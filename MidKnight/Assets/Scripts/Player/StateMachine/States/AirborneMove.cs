using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirMove", menuName = "States/AirMove", order = 2)]
public class AirborneMove : State
{
    public float maxFallSpeed = 0;
    [Tooltip("How long the player can hold the jump key to not be affected by gravity")]
    public float jumpHoldTime = 0;
    [Tooltip("How long the player floats for after 'exiting' a jump early. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float floatTime = 0;
    [Tooltip("How long it takes for the players vertical speed to reach 0. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float timeToZero = 0;
    [Tooltip("The fall speed the player will quickly accelerate too. ONLY APPLIES TO EARLY JUMP RELEASE")]
    [Range(0,100)]
    public float minFallSpeed = 0;
    [Tooltip("The time it takes to go from 0 velocity to min velocity vertically. ONLY APPLIES TO EARLY JUMP RELEASE")]
    public float timeToMinFallSpeed = 0;

    private bool holdingJump = false;
    private bool earlyJumpRelease = false;

    private float jumpTimer = 0;
    private float floatTimer = 0;
    private float decelToZeroTimer = 0;
    private float accelToMin = 0;

    public override void StateStart(ref PlayerController c)
    {
        earlyJumpRelease = false;
        holdingJump = false;
        jumpTimer = 0;
        floatTimer = 0;
        if (Input.GetAxisRaw("Jump") > 0)
        {
            jumpTimer = jumpHoldTime;
            floatTimer = floatTime;
            decelToZeroTimer = timeToZero;
            accelToMin = timeToMinFallSpeed;
            holdingJump = true;
        }
    }

    public override void StateUpdate(ref PlayerController c)
    {   //Get the inputs
        float x = Input.GetAxisRaw("Horizontal");

        if (holdingJump && (Input.GetAxisRaw("Jump") <= 0 || jumpTimer < 0))
            holdingJump = false;
        else if (holdingJump)
            jumpTimer -= Time.deltaTime;

        if (!holdingJump && jumpTimer > 0)
        {
            earlyJumpRelease = true;
            jumpTimer = 0;
        }

        if (earlyJumpRelease)
        {
            //Decellerate the player to 0
            if (c.movement.VertSpeed > 0)
            {
                c.movement.VertSpeed -= (c.movement.VertSpeed / decelToZeroTimer) * Time.deltaTime;
                decelToZeroTimer -= Time.deltaTime;

                if (decelToZeroTimer <= 0)
                    c.movement.VertSpeed = 0;
            }
            //Float for floatTime
            else if (c.movement.VertSpeed == 0)
                floatTimer -= Time.deltaTime;
            //Accelerate to min speed
            if (floatTimer <= 0 && c.movement.VertSpeed <= 0)
            {
                c.movement.VertSpeed -= ((minFallSpeed - c.movement.VertSpeed) / accelToMin) * Time.deltaTime;
                accelToMin -= Time.deltaTime;

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
        Vector3 right = c.gm.GetPathDirection(c.transform.position);
        if (x > 0)
            c.FacingRight = true;
        //Invert the vector if we are trying to go left
        if (x < 0)
        {
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
