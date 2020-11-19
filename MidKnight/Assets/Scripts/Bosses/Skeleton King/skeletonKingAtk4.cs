using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// skeleton king's fourth attack
/// </summary>
public class skeletonKingAtk4 : baseBossAttack
{
    /// <summary>
    /// the start time till attack in phase 2
    /// </summary>
    public float phase2StartTimeTillAtk;
    /// <summary>
    /// the radius of the barrier
    /// </summary>
    int barrierRadius;
    /// <summary>
    /// the radius of the barrier in phase 1
    /// </summary>
    public int phase1BarrierRadius;
    /// <summary>
    /// the radius of the barrier in phase 2
    /// </summary>
    public int phase2BarrierRadius;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //change the radius and and cast speed depending on the phase
        barrierRadius = phase1BarrierRadius;

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            barrierRadius = phase2BarrierRadius;
            timeTillAtk = phase2StartTimeTillAtk;
        }
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
            GameObject barrier = Instantiate(attack, enemyTrans.position, enemyTrans.rotation, enemyTrans.parent);
            barrier.GetComponent<Transform>().localScale = new Vector3(barrierRadius, barrierRadius, barrierRadius);
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
