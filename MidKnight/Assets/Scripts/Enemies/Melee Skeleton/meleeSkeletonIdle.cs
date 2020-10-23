using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSkeletonIdle : baseEnemyIdle
{
    /// <summary>
    /// Melee AND ranged skeleton idle 
    /// </summary>
    
    public int atkRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //atk as soon as skele is in range and player is above skeleton
        if (Vector3.Distance(playerTrans.position, enemyTrans.position) < atkRange && playerTrans.position.y >= enemyTrans.position.y)
        {
            animator.SetTrigger("atk");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //atk as soon as skele is in range and player is above skeleton
        if(Vector3.Distance(playerTrans.position, enemyTrans.position) < atkRange && playerTrans.position.y >= enemyTrans.position.y)
        {
            animator.SetTrigger("atk");
        }

        //walk to the player if there is one, but stop if there is a wall or no floor
        if(PlayerCheck())
        {
            FacePlayer();

            if (WallAndFloorCheck())
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }
            else
            {
                destination.Set(playerTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }
        }

        //skele always walks to destination
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
