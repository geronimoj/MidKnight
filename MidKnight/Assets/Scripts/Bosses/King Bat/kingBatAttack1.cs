using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingBatAttack1 : baseBossAttack
{
    /// <summary>
    /// king bat's first attack
    /// It spawns 2 small bats
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

            //spawns 2 small bats
            for (int i = 0; i < 2; i++)
            {
                spawnPos.Set(Random.Range(arenaLeftXCoordinate, arenaUpYCoordinate), arenaUpYCoordinate - 1f, enemyTrans.position.z);
                Instantiate(attack, spawnPos, enemyTrans.rotation, enemyTrans.parent);
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
