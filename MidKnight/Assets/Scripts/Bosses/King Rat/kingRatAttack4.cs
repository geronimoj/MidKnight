using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// king rat's fourth attack
/// it shoots a laser beam into the sky, and multiple more rain down shortly after
/// </summary>
public class kingRatAttack4 : baseBossAttack
{
    /// <summary>
    /// the number of laser beams
    /// </summary>
    public int noOfLaserBeams;
    /// <summary>
    /// start time till it uses its second attack
    /// </summary>
    public float starttTimeTillAtk2;
    /// <summary>
    /// current time till it uses its second attack
    /// </summary>
    float timeTillAtk2;
    /// <summary>
    /// returns true if its used its second attack
    /// </summary>
    bool hasUsedAtk2;
    /// <summary>
    /// the spawn pos of the laser
    /// </summary>
    Vector3 spawnPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        timeTillAtk2 = starttTimeTillAtk2;
        hasUsedAtk2 = false;
        spawnPos = new Vector3(0, 0, 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeTillAtk2 > 0)
        {
            timeTillAtk2 -= Time.deltaTime;
        }
        else if (!hasUsedAtk2)
        {
            if(isFacingRight())
            {
                spawnPos.Set(enemyTrans.position.x - 2, enemyTrans.position.y + 5, enemyTrans.position.z);
            }
            else
            {
                spawnPos.Set(enemyTrans.position.x + 2, enemyTrans.position.y + 5, enemyTrans.position.z);
            }
            GameObject laser = Instantiate(attack, spawnPos, Quaternion.Euler(90, 0, 0), enemyTrans.parent);
            laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            hasUsedAtk2 = true;
        }


        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if (!hasUsedMove)
        {
            for (int i = 0; i < noOfLaserBeams; i++)
            {
                spawnPos.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), arenaDownYCoordinate, enemyTrans.position.z);
                GameObject laser = Instantiate(attack, spawnPos, Quaternion.Euler(90, 0, 0), enemyTrans.parent);
                laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            hasUsedMove = true;
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
