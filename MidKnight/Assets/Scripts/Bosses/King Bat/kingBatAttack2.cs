using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// king bat's second attack.
/// it dashes to the player
/// </summary>
public class kingBatAttack2 : baseBossAttack
{
    /// <summary>
    /// the amount of times the bat has reached its destination
    /// </summary>
    int count;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        FacePlayer();
        count = 0;
        destination.Set(playerTrans.position.x, playerTrans.position.y + 1, enemyTrans.position.z);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else
        {
            MoveToDestination(destination);

            if(Vector3.Distance(enemyTrans.position,destination) < 0.1f)
            {
                count++;

                if(count == 1)
                {
                    destination.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaDownYCoordinate, arenaUpYCoordinate), enemyTrans.position.z);

                    if(destination.x > enemyTrans.position.x)
                    {
                        FaceRight();
                    }
                    else
                    {
                        FaceLeft();
                    }
                }
                else if(count == 2)
                {
                    animator.SetTrigger("idle");
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
