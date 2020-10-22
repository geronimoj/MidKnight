﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batAttack : StateMachineBehaviour
{
    public int speed;
    Transform sleepRadius;
    public float sleepRadiusSize;
    bool playerCheck;
    Transform batTrans;
    Transform playerTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        batTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Change the radius for the bat to sleep in the inspector
        sleepRadius = animator.gameObject.transform.GetChild(1);
        sleepRadius.localScale = new Vector3(sleepRadiusSize, sleepRadiusSize, sleepRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCheck = animator.GetComponentInChildren<playerCheck>().isTherePlayer;

        //if the player is nearby, chase the player
        if (playerCheck)
        {
            batTrans.position = Vector3.MoveTowards(batTrans.position, playerTrans.position, speed * Time.deltaTime);

            //Always face the player while awake
            if (playerTrans.position.x > batTrans.position.x)
            {
                batTrans.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                batTrans.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
