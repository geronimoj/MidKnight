using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class werewolfAttack3 : baseBossAttack
{
    public int phase2speed;
    bool isMovingRight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }

        if(PlayerOnRight())
        {
            destination.Set(arenaLeftXCoordinate/3, enemyTrans.position.y + 5f, enemyTrans.position.z);
            isMovingRight = true;
        }
        else
        {
            destination.Set(arenaRightXCoordinate/3, enemyTrans.position.y + 5f, enemyTrans.position.z);
            isMovingRight = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector3.Distance(enemyTrans.position, destination) < 0.2f)
        {
            if (isMovingRight)
            {
                destination.Set(arenaRightXCoordinate / 2, enemyTrans.position.y -5f, enemyTrans.position.z);
            }
            else
            {
                destination.Set(arenaLeftXCoordinate / 2, enemyTrans.position.y - 5f, enemyTrans.position.z);
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
