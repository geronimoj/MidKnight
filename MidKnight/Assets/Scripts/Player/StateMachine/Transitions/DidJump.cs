using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DidJump", menuName = "Transitions/DidJump", order = 2)]
public class DidJump : Transition
{
    /// <summary>
    /// Determines if a new input has been made
    /// </summary>
    private bool newInput = false;
    /// <summary>
    /// Checks if the player should jump
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>Returns true on a successful pass</returns>
    public override bool ShouldTransition(ref PlayerController c)
    {
        //Did the player press the jump key
        if (newInput && Input.GetAxis("Jump") > 0 && c.CanJump)
        {   //Assign jump force
            c.movement.VertSpeed = c.OnJumpForce;
            c.OnJump();
            newInput = false;
            c.Audio.PlayOneShot(c.jump);
#if UNITY_EDITOR
            //Debug that we jumped for the editor
            Debug.Log("Jump");
#endif
            return true;
        }

        if (Input.GetAxis("Jump") == 0)
            newInput = true;
        return false;
    }
}
