using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirMove", menuName = "States/AirMove", order = 2)]
public class AirborneMove : State
{
    public float maxFallSpeed = 0;
    [Tooltip("How long the player can hold the jump key to not be affected by gravity")]
    public float jumpHoldTime = 0;
    [Tooltip("How long the player floats for after 'exiting' a jump")]
    public float floatTime = 0;

    private bool holdingJump = false;

    private float jumpTimer = 0;
    private float floatTimer = 0;

    public override void StateStart(ref PlayerController c)
    {
        holdingJump = false;
        jumpTimer = 0;
        floatTimer = 0;
        if (Input.GetAxisRaw("Jump") > 0)
        {
            jumpTimer = jumpHoldTime;
            floatTimer = floatTime;
            holdingJump = true;
        }
    }

    public override void StateUpdate(ref PlayerController c)
    {   //Get the inputs
        float x = Input.GetAxisRaw("Horizontal");

        if (jumpTimer > 0 && holdingJump && Input.GetAxisRaw("Jump") > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        else if (holdingJump)
        {
            c.movement.VertSpeed = 0;
            floatTimer -= Time.deltaTime;
            jumpTimer = 0;
        }

        if (jumpTimer <= 0 && floatTimer <= 0)
            holdingJump = false;

        //Find the players speed
        float speed = 0;
        if (x != 0)
            speed = c.MoveSpeed;
        //Get the direction to the right
        Vector3 right = c.gm.GetPathDirection(c.transform.position);
        //Invert the vector if we are trying to go left
        if (x < 0)
            right = -right;
        //Set the horizontal direction of the player to that direction
        right *= speed;

        c.movement.Direction = right;
        //Set the players speed
        c.movement.HozSpeed = speed;

        if (!holdingJump)
            c.movement.VertSpeed -= c.Gravity * Time.deltaTime;
        //Cap the players fall speed if necessary
        if (maxFallSpeed > 0)
            c.movement.VertSpeed = Mathf.Clamp(c.movement.VertSpeed, -maxFallSpeed, Mathf.Infinity);

        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
    }
}
