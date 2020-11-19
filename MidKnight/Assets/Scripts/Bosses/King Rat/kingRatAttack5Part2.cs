using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// king rat's fifth attack part 2
/// he shoots a laser beam and rains rocks
/// </summary>
public class kingRatAttack5Part2 : baseBossAttack
{
    /// <summary>
    /// the number of rocks to drop
    /// </summary>
    public int noOfRocks;
    /// <summary>
    /// a reference to the rock prefab
    /// </summary>
    public GameObject rocks;
    /// <summary>
    /// the spawn position of each rock
    /// </summary>
    Vector3 spawnPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        //initialise stuff
        FacePlayer();
        spawnPos = new Vector3(0, 0, 0);

        //rain down some rocks
        for (int i = 0; i < noOfRocks; i++)
        {
            spawnPos.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaUpYCoordinate + 5, arenaUpYCoordinate + 20), enemyTrans.position.z);
            Instantiate(rocks, spawnPos, enemyTrans.rotation, enemyTrans.parent);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if(!hasUsedMove)
        {
            Instantiate(attack, enemyTrans.position, enemyTrans.rotation, enemyTrans.parent);
            hasUsedMove = true;
            animator.SetTrigger("atk5");
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
