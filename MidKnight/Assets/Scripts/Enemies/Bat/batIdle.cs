using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batIdle : StateMachineBehaviour
{
    /// <summary>
    /// The bat's idle animation
    /// </summary>
    
    Transform wakeRadius;
    public float wakeRadiusSize;
    bool playerCheck;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Change the radius for the bat to wake up in the inspector
        wakeRadius = animator.gameObject.transform.GetChild(0);
        wakeRadius.localScale = new Vector3(wakeRadiusSize, wakeRadiusSize, wakeRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCheck = animator.GetComponentInChildren<playerCheck>().isTherePlayer;

        //Change to attack animation if the player is nearby
        if(playerCheck)
        {
            animator.SetTrigger("attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
