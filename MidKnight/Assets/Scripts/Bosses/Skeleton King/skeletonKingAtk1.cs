using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// skeleton kings first attack.
/// it shoots a laser beam at the player
/// </summary>
public class skeletonKingAtk1 : baseBossAttack
{
    /// <summary>
    /// the start time till attack in phase 2
    /// </summary>
    public float phase2StartTimeTillAtk;
    /// <summary>
    /// the height of the laser beams
    /// </summary>
    public int laserBeamHeight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //the laser appears earlier if its in phase 2
        if (animator.GetComponent<Enemy>().isPhase2)
        {
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
        else if(!hasUsedMove)
        {
            hasUsedMove = true;

            GameObject laserBeam = Instantiate(attack, enemyTrans.position, enemyTrans.rotation, enemyTrans.parent);
            laserBeam.GetComponent<Transform>().localScale = new Vector3(laserBeamHeight, laserBeamHeight, laserBeamHeight);
            laserBeam.GetComponentInChildren<basePrefab>().damage = 2;
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
