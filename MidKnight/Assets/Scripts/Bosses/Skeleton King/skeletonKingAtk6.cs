using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// skeleton king's sixth attack.
/// it summons a barrier on him
/// </summary>
public class skeletonKingAtk6 : baseBossAttack
{
    /// <summary>
    /// the start time till attack in phase 2
    /// </summary>
    public float phase2StartTimeTillAtk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //the barrier summons earlier if its phase 2
        if (animator.GetComponent<Enemy>().isPhase2)
        {
            timeTillAtk = phase2StartTimeTillAtk;
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
            if(isFacingRight())
            {
                GameObject laser = Instantiate(attack, new Vector3(enemyTrans.position.x + 4, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation, enemyTrans.parent);
                laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
            else
            {
                GameObject laser = Instantiate(attack, new Vector3(enemyTrans.position.x - 4, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation, enemyTrans.parent);
                laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
            hasUsedMove = true;
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
