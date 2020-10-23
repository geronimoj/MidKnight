using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeRatIdle : baseEnemyIdle
{
    /// <summary>
    /// Large rat's idle script
    /// </summary>
     
    bool isMovingRight;
    Transform chaseRadius;
    public float chaseRadiusSize;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // Do custom stuff for the rat
        destination = new Vector3(enemyTrans.position.x + 500, enemyTrans.position.y, enemyTrans.position.z);

        //change the radius of rats vision in inspector
        chaseRadius = animator.gameObject.transform.GetChild(2);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //If the player is nearby, it changes direction to run into the player
        if (PlayerCheck())
        {
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

            if(WallAndFloorCheck())
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }
        }
        else
        {
            //If there's a wall or no floor in front of the rat, it changes directions
            SwapDirections();
        }

        //Move to it's destination
        MoveToDestination(destination);
    }

    //swap the direction its facing
    void SwapDirections()
    {
        if(WallAndFloorCheck())
        {
            if (isMovingRight)
            {
                enemyTrans.eulerAngles = new Vector3(0, 180, 0);
                destination.Set(enemyTrans.position.x - 500, enemyTrans.position.y, enemyTrans.position.z);
                isMovingRight = false;
            }
            else
            {
                enemyTrans.eulerAngles = new Vector3(0, 0, 0);
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
