using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundMove", menuName = "States/GroundMove", order = 1)]
public class GroundMove : State
{
    public override void StateStart(ref PlayerController c)
    {   //Remove the players vertical speed
        c.movement.VertSpeed = 0;
    }

    public override void StateUpdate(ref PlayerController c)
    {
        float x = Input.GetAxisRaw("Horizontal");
        //Don't move at all if there is no input
        if (x == 0)
            return;

        //Set the direction
        c.movement.Direction = c.gm.GetPathDirection(c.transform.position);
        if (x > 0)
            c.FacingRight = true;
        //Invert the direction if we are walking left
        if (x < 0)
        {
            c.FacingRight = false;
            c.movement.Direction = -c.movement.Direction;
        }

        //Set the players move speed
        c.movement.HozSpeed = c.MoveSpeed;
        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
    }
}
