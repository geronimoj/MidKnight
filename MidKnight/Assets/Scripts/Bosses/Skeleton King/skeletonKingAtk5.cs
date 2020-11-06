using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonKingAtk5 : baseBossAttack
{
    public float phase2StartTimeTillAtk;
    public int phase2Speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

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
        else if (!hasUsedMove)
        {
            hasUsedMove = true;
            
            if(enemyTrans.position.x > playerTrans.position.x)
            {
                GameObject meteor = Instantiate(attack, new Vector3(arenaRightXCoordinate, arenaUpYCoordinate, enemyTrans.position.z), enemyTrans.rotation);

                if (animator.GetComponent<Enemy>().isPhase2)
                {
                    meteor.GetComponent<skeletonKingMeteor>().speed = phase2Speed;
                }
            }
            else
            {
                GameObject meteor = Instantiate(attack, new Vector3(arenaLeftXCoordinate, arenaUpYCoordinate, enemyTrans.position.z), enemyTrans.rotation);

                if (animator.GetComponent<Enemy>().isPhase2)
                {
                    timeTillAtk = phase2StartTimeTillAtk;
                    meteor.GetComponent<skeletonKingMeteor>().speed = phase2Speed;
                }
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
