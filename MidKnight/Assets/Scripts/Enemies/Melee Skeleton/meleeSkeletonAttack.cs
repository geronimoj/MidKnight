using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSkeletonAttack : baseEnemyAttack
{
    /// <summary>
    /// the attack that the skeleton spawns
    /// </summary>
    public GameObject attack;
    /// <summary>
    /// the position where the skeleton spawns its attack
    /// </summary>
    Vector3 spawnPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);   

        if(PlayerOnRight())
        {
            spawnPos = new Vector3(enemyTrans.position.x + 1f, enemyTrans.position.y +1, enemyTrans.position.z);
        }
        else
        {
            spawnPos = new Vector3(enemyTrans.position.x - 1f, enemyTrans.position.y +1, enemyTrans.position.z);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if(!hasUsedAtk)
        {
            hasUsedAtk = true;
            Instantiate(attack, spawnPos, enemyTrans.rotation, enemyTrans.parent);
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
