using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "States/Dash", order = 3)]
public class Dash : State
{
    /// <summary>
    /// The distance the dash will cover
    /// </summary>
    [Range(0.001f, 100)]
    public float distance = 1;
    /// <summary>
    /// The duration of the dash
    /// </summary>
    [Range(0.001f, 10)]
    public float duration = 0.1f;
    /// <summary>
    /// The bonus distance of the dash during a half moon
    /// </summary>
    [Range(0.001f, 100)]
    public float halfMoonBonusDist = 1;
    /// <summary>
    /// A storage location for the speed of the dash
    /// </summary>
    private float dashSpeed = 0;
    /// <summary>
    /// A timer for the dash so we don't overshoot the time
    /// </summary>
    private float dashTimer = 0;

    public override void StateStart(ref PlayerController c)
    {   //Make sure we have space for the transition
        if (ignoreTransitions.Length < 1)
            ignoreTransitions = new bool[1];
        //If we have half moon unlocked, gain bonus dash distance
        if (c.CurrentPhaseIDCompare("Half Moon"))
            dashSpeed = (distance + halfMoonBonusDist) / duration;
        else
            //Calculate the speed of the dash
            dashSpeed = distance / duration;
        //No vertical movement
        c.movement.VertSpeed = 0;
        //Disable the OnGround/InAir transition
        ignoreTransitions[0] = true;
        //Set the tiemr
        dashTimer = duration;
        //Set the direction of movement for the dash
        c.movement.Direction = c.gm.GetPathDirectionRight(c.transform.position);
        if (!c.FacingRight)
            c.movement.Direction = -c.movement.Direction;

        c.animator.SetBool("Dashing", true);
    }

    public override void StateUpdate(ref PlayerController c)
    {   //Set the horizontal speed
        c.movement.HozSpeed = dashSpeed;
        //Move the player & decrement the timer
        c.Move(c.movement.MoveVec * Time.deltaTime);
        dashTimer -= Time.deltaTime;
        //If we are done dashing, enable the OnGround_Else_Airborne transition
        if (dashTimer <= 0)
            ignoreTransitions[0] = false;
    }

    public override void StateEnd(ref PlayerController c)
    {
        c.animator.SetBool("Dashing", false);
    }
}
