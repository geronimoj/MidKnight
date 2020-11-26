using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// werewolf's fourth attack. it jumps high in the air and lands on the player
/// </summary>
public class werewolfAttack4 : baseBossAttack
{
    /// <summary>
    /// the speed in phase 2
    /// </summary>
    public int phase2speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        //set him to have 0 gravity
        gravity = 0;

        destination.Set(playerTrans.position.x, arenaUpYCoordinate, enemyTrans.position.z);

        //change the speed if its phase 2
        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //change its gravity to make it fall faster
        if(Vector3.Distance(enemyTrans.position, destination) < 0.2f)
        {
            destination.Set(enemyTrans.position.x, arenaDownYCoordinate + 1, enemyTrans.position.z);

            gravity = 30;

            if (Vector3.Distance(enemyTrans.position, destination) < 0.2f)
            {
                animator.SetTrigger("idle");
            }
        }

        MoveToDestination(destination);  


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
