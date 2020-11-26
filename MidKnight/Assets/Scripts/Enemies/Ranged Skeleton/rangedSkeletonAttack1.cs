using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the script for the laser beam attack animation
/// </summary>
public class rangedSkeletonAttack1 : baseEnemyAttack
{
    /// <summary>
    /// a reference to the ranged skeleton's first attack
    /// </summary>
    public GameObject atk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //after this many seconds, spawn the attack once at the location of the enemy
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if (!hasUsedAtk)
        {
            hasUsedAtk = true;

            Vector3 spawnPos = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 1.3f, enemyTrans.position.z);

            if(PlayerOnRight())
            {
                GameObject laser = Instantiate(atk, spawnPos, Quaternion.Euler(180, 90, 0), enemyTrans.parent);
                laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
            else
            {
                GameObject laser = Instantiate(atk, spawnPos, Quaternion.Euler(0, 90, 0), enemyTrans.parent);
                laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
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
