using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoDash", menuName = "Transitions/DoDash", order = 3)]
public class DoDash : Transition
{
    public override bool ShouldTransition(ref PlayerController c)
    {   //Check if the player can dash and if they pressed the dash key
        if (c.CanDash && Input.GetAxis("Dash") > 0)
        {   //Log that we dashed
#if UNITY_EDITOR
            Debug.Log("Dash");
#endif      //Let the player controller know that we dashed
            c.DoDash();
            return true;
        }
        return false;
    }
}
