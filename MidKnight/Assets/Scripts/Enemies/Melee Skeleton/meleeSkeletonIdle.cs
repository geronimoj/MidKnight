﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSkeletonIdle : StateMachineBehaviour
{
    public float minStartTimeTillMove;
    public float maxStartTimeTillMove;
    float timeTillMove;
    int dirToWalk;
    Transform skeleTrans;
    public int speed;
    Vector3 destination;
    bool hasChosenDir;
    float distToMove;
    public float maxDistToMove;
    public float minDistToMove;
    bool floorCheck;
    bool wallCheck;
    bool playerCheck;
    Transform chaseRadius;
    public float chaseRadiusSize;
    Transform playerTrans;
    public int atkRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasChosenDir = false;
        timeTillMove = Random.Range(minStartTimeTillMove, maxStartTimeTillMove);
        skeleTrans = animator.GetComponent<Transform>();
        destination = new Vector3(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
        chaseRadius = animator.gameObject.transform.GetChild(2);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        floorCheck = animator.GetComponentInChildren<floorCheck>().isThereFloor;
        wallCheck = animator.GetComponentInChildren<wallCheck>().isThereAWall;
        playerCheck = animator.GetComponentInChildren<playerCheck>().isTherePlayer;

        if (timeTillMove > 0)
        {
            timeTillMove -= Time.deltaTime;
        }
        else if(timeTillMove < 0 && !hasChosenDir)
        {
            hasChosenDir = true;
            dirToWalk = Random.Range(1, 4);
            distToMove = Random.Range(minDistToMove, maxDistToMove);
        }


        if(dirToWalk == 1)
        {
            skeleTrans.eulerAngles = new Vector3(0, 180, 0);

            destination.Set(skeleTrans.position.x + distToMove, skeleTrans.position.y, skeleTrans.position.z);

            if (wallCheck || !floorCheck)
            {
                destination.Set(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
            }
        }
        else if(dirToWalk == 2)
        {
            skeleTrans.eulerAngles = new Vector3(0, 0, 0);

            destination.Set(skeleTrans.position.x - distToMove, skeleTrans.position.y, skeleTrans.position.z);

            if (wallCheck || !floorCheck)
            {
                destination.Set(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
            }
        }

        skeleTrans.position = Vector3.MoveTowards(skeleTrans.position, destination, speed * Time.deltaTime);

        if (playerCheck)
        {
            if(playerTrans.position.x > skeleTrans.position.x)
            {
                destination.Set(playerTrans.position.x - atkRange, skeleTrans.position.y, skeleTrans.position.z);

                if (wallCheck || !floorCheck)
                {
                    destination.Set(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
                }
            }
            else
            {
                destination.Set(playerTrans.position.x + atkRange, skeleTrans.position.y, skeleTrans.position.z);

                if (wallCheck || !floorCheck)
                {
                    destination.Set(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
                }
            }

            if(skeleTrans.position == destination)
            {
                animator.SetTrigger("attack");
            }
        }
        else
        {
            if (skeleTrans.position == destination)
            {
                hasChosenDir = false;
                timeTillMove = Random.Range(minStartTimeTillMove, maxStartTimeTillMove);
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
