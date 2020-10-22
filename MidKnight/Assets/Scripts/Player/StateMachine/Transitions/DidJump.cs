using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DidJump", menuName = "Transitions/DidJump", order = 2)]
public class DidJump : Transition
{
    public override bool ShouldTransition(ref PlayerController c)
    {
        if (Input.GetAxis("Jump") > 0)
        {
            c.movement.VertSpeed = c.OnJumpForce;
#if UNITY_EDITOR
            Debug.Log("Jump");
#endif
            return true;
        }
        
        return false;
    }
}
