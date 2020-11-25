using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// werewolf's first attack.
/// it summons rocks from the sky
/// </summary>
public class werewolfAttack1 : baseBossAttack
{
    /// <summary>
    /// number of rocks to spawn
    /// </summary>
    public int noOfRocks;
    /// <summary>
    /// the spawn position of each rock
    /// </summary>
    Vector3 spawnPos;
    /// <summary>
    /// the speed in phase 2
    /// </summary>
    public int phase2speed;
    /// <summary>
    /// the size of the rocks
    /// </summary>
    public float rockRadius;
    /// <summary>
    /// the vector3 to set the rock size
    /// </summary>
    Vector3 rockRadiusVector;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set the destination to the middle of the arena
        destination.Set((arenaLeftXCoordinate + arenaRightXCoordinate) / 2, enemyTrans.position.y + 0.1f, enemyTrans.position.z);
        
        //change its speed if its in phase 2.
        if(animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }

        rockRadiusVector = new Vector3(rockRadius, rockRadius, rockRadius);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

        if (Vector3.Distance(enemyTrans.position, destination) < 0.2f && !hasUsedMove)
        {
            hasUsedMove = true;

            //summon rocks
            for (int i = 0; i < noOfRocks; i++)
            {
                spawnPos = new Vector3(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaUpYCoordinate + 5, arenaUpYCoordinate + 20), enemyTrans.position.z);
                GameObject rock = Instantiate(attack, spawnPos, enemyTrans.rotation, enemyTrans.parent);
                rock.GetComponent<Transform>().localScale = rockRadiusVector;
            }

            animator.SetTrigger("idle");
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
