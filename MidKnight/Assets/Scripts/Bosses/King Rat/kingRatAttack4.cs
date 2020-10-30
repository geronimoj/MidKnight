﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingRatAttack4 : baseBossAttack
{
    public int noOfLaserBeams;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameObject laserbeam = Instantiate(attack, enemyTrans.position, Quaternion.Euler(0, 0, 90));
        characterOwner co = laserbeam.GetComponent<characterOwner>();
        Debug.Assert(co != null, "Did not find characterOwner script on spawned prefab");
        co.Owner = animator.gameObject;
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
                GameObject laserbeam = Instantiate(attack, spawnPos , Quaternion.Euler(0, 0, 90));
                characterOwner co = laserbeam.GetComponent<characterOwner>();
                Debug.Assert(co != null, "Did not find characterOwner script on spawned prefab");
                co.Owner = animator.gameObject;
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