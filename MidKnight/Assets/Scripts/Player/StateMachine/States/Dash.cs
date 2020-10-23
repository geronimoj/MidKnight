using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "States/Dash", order = 3)]
public class Dash : State
{
    [Range(0.001f, 100)]
    public float distance = 1;

    [Range(0.001f, 10)]
    public float duration = 0.1f;

    private float dashSpeed = 0;
    private float dashTimer = 0;

    public override void StateStart(ref PlayerController c)
    {
        if (ignoreTransitions.Length < 1)
            ignoreTransitions = new bool[1];

        dashSpeed = distance / duration;
        c.movement.VertSpeed = 0;
        ignoreTransitions[0] = true;
        dashTimer = duration;

        c.movement.Direction = c.gm.GetPathDirection(c.transform.position);
        if (!c.FacingRight)
            c.movement.Direction = -c.movement.Direction;
    }

    public override void StateUpdate(ref PlayerController c)
    {
        c.movement.HozSpeed = dashSpeed;
        c.Move(c.movement.MoveVec * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
            ignoreTransitions[0] = false;
    }
}
