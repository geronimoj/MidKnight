using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirMove", menuName = "States/AirMove", order = 2)]
public class AirborneMove : State
{
    public override void StateUpdate(ref PlayerController c)
    {   //Gravity
        c.movement.VertSpeed -= c.Gravity * Time.deltaTime;
        //Get the input
        float x = Input.GetAxisRaw("Horizontal");
        //Check if the player should move horiztonally
        if (x == 0)
            c.movement.HozSpeed = 0;
        else
            c.movement.HozSpeed = c.MoveSpeed;
        //Update the movement direction
        c.movement.Direction = c.gm.GetPathDirection(c.transform.position);
        //If we are moving left, invert the direction
        if (x < 0)
            c.movement.Direction = -c.movement.Direction;
        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
    }
}
