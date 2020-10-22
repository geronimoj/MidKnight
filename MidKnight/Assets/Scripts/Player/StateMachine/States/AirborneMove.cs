using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirMove", menuName = "States/AirMove", order = 2)]
public class AirborneMove : State
{
    public override void StateUpdate(ref PlayerController c)
    {   //Get the input
        float x = Input.GetAxisRaw("Horizontal");
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
        c.movement.VertSpeed -= c.Gravity * Time.deltaTime;
        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
    }
}
