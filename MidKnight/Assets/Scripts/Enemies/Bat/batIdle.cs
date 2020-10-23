using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batIdle : baseEnemyIdle
{
    /// <summary>
    /// The bat's idle animation
    /// </summary>
    
    Transform chaseRadius;
    public float chaseRadiusSize;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //custom stuff for bat
        //change the radius of bats vision in inspector
        chaseRadius = animator.gameObject.transform.GetChild(0);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Change to attack animation if the player is nearby
        if(PlayerCheck())
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
