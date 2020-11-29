using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundMove", menuName = "States/GroundMove", order = 1)]
public class GroundMove : State
{   
    /// <summary>
    /// The time to accelerate to the max speed
    /// </summary>
    [Range(0,100)]
    public float accelToMaxTime = 0;
    /// <summary>
    /// The time it takes to decellerate to zero when releasing the moveKey
    /// </summary>
    [Range(0, 100)]
    public float decelToZeroTime = 0;

    private float accelTimer = 0;
    private float decelTimer = 0;

    public override void StateStart(ref PlayerController c)
    {   //Remove the players vertical speed
        c.movement.VertSpeed = 0;
        c.animator.SetBool("Airborne",false);
        accelTimer = 0;
        decelTimer = 0;
        c.SafePoint = c.transform.position;
        c.OnLand();
    }

    public override void StateUpdate(ref PlayerController c)
    {   //Get the input
        float x = Input.GetAxisRaw("Horizontal");
        //Update the safe point if the point is safe
        if (!c.Attacking && !c.InHitStun)
        {
            c.SafePoint = c.transform.position;
        }
        //Don't move at all if there is no input
        if (x == 0)
        {
            c.movement.HozSpeed = LerpTo(0, c.movement.HozSpeed, ref decelTimer);
            if (decelToZeroTime != 0)
                accelTimer = (1 - (decelTimer / decelToZeroTime)) * accelToMaxTime;
            c.Walk.Stop();
        }
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
            c.movement.HozSpeed = LerpTo(c.MoveSpeed, c.movement.HozSpeed, ref accelTimer);
            if (accelToMaxTime != 0)
                decelTimer = (1 - (accelTimer / accelToMaxTime)) * decelToZeroTime;
            if (!c.Walk.isPlaying)
                c.Walk.PlayOneShot(c.walk);
        }
        //Move the character
        c.Move(c.movement.MoveVec * Time.deltaTime);
        c.animator.SetFloat("MoveSpeed", c.movement.HozSpeed);
    }
    /// <summary>
    /// Transitions current to target over the remaining timer
    /// </summary>
    /// <param name="target">Target float</param>
    /// <param name="current">Current float</param>
    /// <param name="timer">A reference to a timer</param>
    /// <returns>Returns the new current speed</returns>
    private float LerpTo(float target, float current, ref float timer)
    {
        if (timer <= 0)
            return target;
        float dif = target - current;
        float t = Time.deltaTime;

        current += dif * (t / timer);

        timer -= t;

        return current;
    }
}
