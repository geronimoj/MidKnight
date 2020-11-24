using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Baby rat's idle script
/// </summary>
public class ratIdle : baseEnemyIdle
{
    /// <summary>
    /// returns true if the rat is moving to the right
    /// </summary>
    bool isMovingRight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //rat can start off walking left or right
        int coinFlip = Random.Range(1, 3);

        if(coinFlip == 1)
        {
            FaceRight();
            isMovingRight = true;
            destination = new Vector3(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);
        }
        else
        {
            FaceLeft();
            isMovingRight = false;
            destination = new Vector3(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //swap directions when they run into the wall or the platform ends
        SwapDirections();

        //Move to it's destination
        MoveToDestination(destination);

        FacePlayer(isMovingRight);
    }

    //If there's a wall or no floor in front of the rat, it changes directions
    void SwapDirections()
    {
        if (WallAndFloorCheck())
        {
            if (isMovingRight)
            {
                FacePlayer(false);
                destination.Set(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = false;
            }
            else
            {
                FacePlayer(true);
                destination.Set(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = true;
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
