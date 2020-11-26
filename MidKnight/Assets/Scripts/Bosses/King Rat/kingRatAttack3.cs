using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// king rat's third attack
/// it shoots a laser beam
/// </summary>
public class kingRatAttack3 : baseBossAttack
{
    /// <summary>
    /// the spawn position of the laser
    /// </summary>
    Vector3 spawnPos;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if (!hasUsedMove)
        {
            hasUsedMove = true;

            if(isFacingRight())
            {
                spawnPos = new Vector3(enemyTrans.position.x - 3, enemyTrans.position.y + 3, enemyTrans.position.z);
            }
            else
            {
                spawnPos = new Vector3(enemyTrans.position.x + 3, enemyTrans.position.y + 3, enemyTrans.position.z);
            }

            GameObject laser = Instantiate(attack, spawnPos, enemyTrans.rotation, enemyTrans.parent);
            laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
