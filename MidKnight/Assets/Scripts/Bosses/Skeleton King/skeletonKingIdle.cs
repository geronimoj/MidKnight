using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonKingIdle : baseBossIdle
{
    bool hasUsedBossMove = false;
    public int phase2speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }

        if (!hasUsedBossMove && enemy.Health <= 1 / 2 * enemy.MaxHealth)
        {
            moveToUse = 7;
            hasUsedBossMove = true;
        }

        //test 
        moveToUse = 3;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FacePlayer();

        destination.Set(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z);
        MoveToDestination(destination);

        base.OnStateUpdate(animator, stateInfo, layerIndex);
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
