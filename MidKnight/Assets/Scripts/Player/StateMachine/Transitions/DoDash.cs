using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoDash", menuName = "Transitions/DoDash", order = 3)]
public class DoDash : Transition
{
    public override bool ShouldTransition(ref PlayerController c)
    {
        if (c.CanDash && Input.GetAxis("Dash") > 0)
        {
            Debug.Log("Dash");
            c.DoDash();
            return true;
        }
        return false;
    }
}
