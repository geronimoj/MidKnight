﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingRatAttack5 : baseBossAttack
{
    public int noOfDashesToUse;
    int dashCount = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        dashCount++;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dashCount < noOfDashesToUse)
        {
            if (timeTillAtk > 0)
            {
                timeTillAtk -= Time.deltaTime;
            }
            else if (!hasUsedMove)
            {
                hasUsedMove = true;

                if (isFacingRight())
                {
                    destination.Set(arenaRightXCoordinate, enemyTrans.position.y, enemyTrans.position.z);
                }
                else
                {
                    destination.Set(arenaLeftXCoordinate, enemyTrans.position.y, enemyTrans.position.z);
                }
            }
            else if(Mathf.Abs(enemyTrans.position.x - destination.x) > 1)
            {
                MoveToDestination(destination);
            }
            else
            {
                animator.SetTrigger("atk5part2");
            }
        }

        if(dashCount == noOfDashesToUse)
        {
            dashCount = 0;
            animator.SetTrigger("idle");
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