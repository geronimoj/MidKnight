using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonKingAtk3 : baseBossAttack
{
    /// <summary>
    /// the skeleton king's third attack
    /// it summons some laser beams
    /// </summary>
    public float phase2StartTimeTillAtk;
    int noOfLaserBeams;
    public int phase1NoOfLaserBeams;
    public int phase2NoOfLaserBeams;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //the amount of laser beams changes dependong on the phase
        noOfLaserBeams = phase1NoOfLaserBeams;

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            timeTillAtk = phase2StartTimeTillAtk;
            noOfLaserBeams = phase2NoOfLaserBeams;
        }

        if (isFacingRight())
        {
            Instantiate(attack, new Vector3(enemyTrans.position.x + 3, enemyTrans.position.y - 2, enemyTrans.position.z), Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(attack, new Vector3(enemyTrans.position.x - 3, enemyTrans.position.y - 2, enemyTrans.position.z), Quaternion.Euler(0, 0, 90));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if (!hasUsedMove)
        {
            hasUsedMove = true;

            for (int i = 0; i < noOfLaserBeams; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), arenaDownYCoordinate, enemyTrans.position.z);
                Instantiate(attack, spawnPos, Quaternion.Euler(0, 0, 90));
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
