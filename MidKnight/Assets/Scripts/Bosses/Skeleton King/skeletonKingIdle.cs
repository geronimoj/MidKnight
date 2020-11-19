using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the skeleton king's idle animation
/// </summary>
public class skeletonKingIdle : baseBossIdle
{
    /// <summary>
    /// returns true if its used its boss move
    /// </summary>
    bool hasUsedBossMove = false;
    /// <summary>
    /// the speed when the boss is in phase 2
    /// </summary>
    public int phase2speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //change its speed if its phase 2
        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }

        //use this once when its under half health
        if (!hasUsedBossMove && enemy.Health <= 1 / 2 * enemy.MaxHealth)
        {
            moveToUse = 7;
            hasUsedBossMove = true;
        }
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
