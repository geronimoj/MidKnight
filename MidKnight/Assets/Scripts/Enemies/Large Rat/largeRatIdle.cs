﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeRatIdle : baseEnemyIdle
{
    /// <summary>
    /// Large rat's idle script
    /// </summary>
     
    bool isMovingRight;
    public int chaseSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //rat can start off walking left or right
        int coinFlip = Random.Range(1, 3);

        if (coinFlip == 1)
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

        //If the player is nearby, it changes direction to run into the player
        if (PlayerCheck())
        {
            //face the player if hes in range and run towards it
            FacePlayer();

            if (PlayerOnRight())
            {
                destination.Set(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = true;
            }
            else
            {
                destination.Set(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = false;
            }

            //stop if theres a wall or the platform ends
            if(WallAndFloorCheck())
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }

            //move to the player faster
            MoveToDestination(destination, chaseSpeed);
        }
        else
        {
            //If there's a wall or no floor in front of the rat, it changes directions
            SwapDirections();

            //Move to it's destination
            MoveToDestination(destination);
        }
    }

    //swap the direction its facing
    void SwapDirections()
    {
        if (WallAndFloorCheck())
        {
            if (isMovingRight)
            {
                FaceLeft();
                destination.Set(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = false;
            }
            else
            {
                FaceRight();
                destination.Set(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = true;
            }
        }
        else
        {
            if (isMovingRight)
            {
                FaceRight();
                destination.Set(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);
            }
            else
            {
                FaceLeft();
                destination.Set(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
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
