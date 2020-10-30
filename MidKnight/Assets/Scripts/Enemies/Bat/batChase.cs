using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batChase : baseEnemyIdle
{
    /// <summary>
    /// the bat's attack
    /// </summary>

    bool isLargeBat;
    public float distFromPlayerToAtk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //custom stuff for bat

        //check if this is bat or large bat
        if(animator.name == "Large Bat")
        {
            isLargeBat = true;
        }
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if the player is nearby, chase the player
        if (PlayerCheck())
        {
            //Always move to destination
            destination.Set(playerTrans.position.x, playerTrans.position.y, enemyTrans.position.z);
            MoveToDestination(destination);
            FacePlayer();
        }

        //do extra things if it's a large bat
        if(isLargeBat)
        {
            if(Vector3.Distance(enemyTrans.position, playerTrans.position) < distFromPlayerToAtk)
            {
                int rand = Random.Range(1, 3);

                if(rand == 1)
                {
                    animator.SetTrigger("attack1");
                }
                else
                {
                    animator.SetTrigger("attack2");
                }
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
