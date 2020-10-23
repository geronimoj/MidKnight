using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnGround_Else_Airborne", menuName = "Transitions/OnGround_Else_Airborne", order = 1)]
public class OnGround_Else_Airborne : If_ElseTransition
{
    public override bool IfTransition(ref PlayerController c)
    {   //Raycast down to check for ground
        if (Physics.SphereCast(c.transform.position, c.PlayerRadius - 0.01f, Vector3.down, out RaycastHit hit, (c.Height / 2) + 0.01f))
        {   //Since we aren't accounting for the players radius in the previous raycast, we make sure the player is close enough now
            //This is so the capsual will float over ledges like its a cylinder with a flat bottom
            if (c.transform.position.y - hit.point.y > (c.Height / 2) + 0.02f)
                return false;
            //Store the players position so we don't have to read the x and z values later
            Vector3 pos = c.transform.position;
            //Change the y value
            pos.y = hit.point.y + c.Height / 2;
            //Set the players position
            c.transform.position = pos;
            return true;
        }
        //We didn't detect ground so we must be airborne
        return false;
    }
}
