using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingBatAttack4 : baseBossAttack
{
    /// <summary>
    /// king bat's fourth attack.
    /// it flies up and drops rocks at the player
    /// </summary>
    public int noOfRocks;
    Vector3 spawnPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //initialise stuff
        destination.Set(enemyTrans.position.x, arenaUpYCoordinate + 5, enemyTrans.position.z);
        spawnPos = new Vector3(0, 0, 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

        //if the bat is at its destination it drops rocks then moves back down
        if(Vector3.Distance(enemyTrans.position, destination) < 0.1f)
        {
            if (!hasUsedMove)
            {
                destination.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaDownYCoordinate, arenaUpYCoordinate), enemyTrans.position.z);

                hasUsedMove = true;
                for (int i = 0; i < noOfRocks; i++)
                {
                    spawnPos.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaUpYCoordinate + 5, arenaUpYCoordinate + 20), enemyTrans.position.z);
                    Instantiate(attack, spawnPos, enemyTrans.rotation, enemyTrans.parent);
                }
            }
            else
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
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
