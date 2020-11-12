using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class werewolfAttack2 : baseBossAttack
{
    /// <summary>
    /// werewolfs second attack
    /// it runs to the player then attacks him
    /// </summary>
    public int phase2speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //face the player
        if(PlayerOnRight())
        {
            destination.Set(playerTrans.position.x - 2, enemyTrans.position.y, enemyTrans.position.z);
        }
        else
        {
            destination.Set(playerTrans.position.x + 2, enemyTrans.position.y, enemyTrans.position.z);

        }

        //change its speed if its phase 2
        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

        //attack the player when in range
        if(Vector3.Distance(enemyTrans.position, destination) < 0.1f)
        {
            animator.SetTrigger("atk2part2");
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
