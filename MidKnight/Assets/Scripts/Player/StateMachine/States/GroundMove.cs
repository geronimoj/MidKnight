﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundMove", menuName = "States/GroundMove", order = 1)]
public class GroundMove : State
{
    public override void StateStart(ref PlayerController c)
    {   //Remove the players vertical speed
        c.movement.VertSpeed = 0;
        c.animator.SetBool("Airborne",false);
    }

    public override void StateUpdate(ref PlayerController c)
    {   //Get the input
        float x = Input.GetAxisRaw("Horizontal");
        //Don't move at all if there is no input
        if (x == 0)
            c.movement.HozSpeed = 0;
        else
        {
            //Set the direction
            c.movement.Direction = c.gm.GetPathDirectionRight(c.transform.position);
            //Are we looking to the right
            if (x > 0)
                c.FacingRight = true;
            //Invert the direction if we are walking left
            if (x < 0)
            {   //Must be facing left
                c.FacingRight = false;
                c.movement.Direction = -c.movement.Direction;
            }

            //Set the players move speed
            c.movement.HozSpeed = c.MoveSpeed;
        }
        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
        c.animator.SetFloat("MoveSpeed", c.movement.HozSpeed);
    }
}
