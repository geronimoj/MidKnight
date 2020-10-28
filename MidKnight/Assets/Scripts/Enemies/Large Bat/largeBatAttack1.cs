using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeBatAttack1 : baseEnemyIdle
{
    public float windUpDist;
    public float startTimeTillDash;
    float timeTillDash;
    bool hasUsedMove;
    public int dashSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);


        //initialise stuff
        timeTillDash = startTimeTillDash;
        hasUsedMove = false;

        if(PlayerOnRight())
        {
            destination.Set(enemyTrans.position.x - windUpDist, enemyTrans.position.y, enemyTrans.position.z);
        }
        else
        {
            destination.Set(enemyTrans.position.x + windUpDist, enemyTrans.position.y, enemyTrans.position.z);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(timeTillDash > 0)
        {
            timeTillDash -= Time.deltaTime;
            MoveToDestination(destination);
        }
        else if(!hasUsedMove)
        {
            hasUsedMove = true;
            destination.Set(playerTrans.position.x, playerTrans.position.y, enemyTrans.position.z);
        }
        else
        {
            MoveToDestination(destination, dashSpeed);
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
