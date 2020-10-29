﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedSkeletonAttack1 : baseEnemyAttack
{
    /// <summary>
    /// the script for the laser beam attack animation
    /// </summary>

    public GameObject atk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
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

            GameObject projectile = Instantiate(atk, enemyTrans.position, enemyTrans.rotation);
            characterOwner co = projectile.GetComponent<characterOwner>();
            Debug.Assert(co != null, "Did not find characterOwner script on spawned prefab");
            co.Owner = animator.gameObject;
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