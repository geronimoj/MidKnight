using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingRatAttack4 : baseBossAttack
{
    /// <summary>
    /// king rat's fourth attack
    /// it shoots a laser beam into the sky, and multiple more rain down shortly after
    /// </summary>
    public int noOfLaserBeams;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Instantiate(attack, enemyTrans.position, Quaternion.Euler(0, 0, 90), enemyTrans.parent);
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

            for (int i = 0; i < noOfLaserBeams; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), arenaDownYCoordinate, enemyTrans.position.z);
                Instantiate(attack, spawnPos, Quaternion.Euler(0, 0, 90), enemyTrans.parent);
            }
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
