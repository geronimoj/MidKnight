using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratIdle : StateMachineBehaviour
{
    /// <summary>
    /// Baby rat's idle script
    /// </summary>

    Transform ratTrans;
    Vector3 destination;
    public int speed;
    floorCheck floorCheck;
    wallCheck wallCheck;
    bool isMovingRight = true;
    bool isThereFloor;
    bool isThereAWall;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        ratTrans = animator.GetComponent<Transform>();
        floorCheck = animator.GetComponentInChildren<floorCheck>();
        wallCheck = animator.GetComponentInChildren<wallCheck>();
        destination = new Vector3(ratTrans.position.x + 500, ratTrans.position.y, ratTrans.position.z);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //check if wall and floor nearby
        isThereFloor = floorCheck.isThereFloor;
        isThereAWall = wallCheck.isThereAWall;

        //If there's a wall or no floor in front of the rat, it changes directions
        bool wallAndFloorCheck = WallAndFloorCheck();

        if(wallAndFloorCheck)
        {
            if (isMovingRight)
            {
                ratTrans.eulerAngles = new Vector3(0, 180, 0);
                destination.Set(ratTrans.position.x - 500, ratTrans.position.y, ratTrans.position.z);
                isMovingRight = false;
            }
            else
            {
                ratTrans.eulerAngles = new Vector3(0, 0, 0);
                destination.Set(ratTrans.position.x + 500, ratTrans.position.y, ratTrans.position.z);
                isMovingRight = true;
            }
        }

        //Move to it's destination
        ratTrans.position = Vector3.MoveTowards(ratTrans.position, destination, speed * Time.deltaTime);   
    }

    //check for walls and floor
    bool WallAndFloorCheck()
    {
        if (isThereAWall || !isThereFloor)
        {
            return true;
        }
        else
        {
            return false;
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
