using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DidJump", menuName = "Transitions/DidJump", order = 2)]
public class DidJump : Transition
{
    /// <summary>
    /// Checks if the player should jump
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>Returns true on a successful pass</returns>
    public override bool ShouldTransition(ref PlayerController c)
    {
        //Did the player press the jump key
        if (Input.GetAxis("Jump") > 0)
        {   //Assign jump force
            c.movement.VertSpeed = c.OnJumpForce;
#if UNITY_EDITOR
            //Debug that we jumped for the editor
            Debug.Log("Jump");
#endif
            return true;
        }
        
        return false;
    }
}
