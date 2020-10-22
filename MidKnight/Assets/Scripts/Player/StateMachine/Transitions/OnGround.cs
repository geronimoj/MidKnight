using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OnGround", menuName = "Transitions/OnGround", order = 1)]
public class OnGround : Transition
{
    public override bool ShouldTransition(ref PlayerController c)
    {
        if (Physics.Raycast(c.transform.position, Vector3.down, out RaycastHit hit, (c.Height / 2) + (c.movement.VertSpeed * Time.deltaTime) + 0.01f))
        {   //Teleport the player onto the ground
            c.transform.position = hit.point + Vector3.up * (c.Height / 2);
            return true;
        }
        return false;
    }
}
