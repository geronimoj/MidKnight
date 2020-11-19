using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the skeleton king's first attack
/// it summons homing orbs
/// </summary>
public class skeletonKingAtk2 : baseBossAttack
{
    /// <summary>
    /// the start time till attack in phase 2
    /// </summary>
    public float phase2StartTimeTillAtk;
    /// <summary>
    /// the current time between each attack
    /// </summary>
    float timeBetweenAttacks;
    /// <summary>
    /// the start time between each attack
    /// </summary>
    public float startTimeBetweenAttacks;
    /// <summary>
    /// the number of orbs to spawn
    /// </summary>
    int noOfOrbs;
    /// <summary>
    /// the number of orbs spawned
    /// </summary>
    int count;
    /// <summary>
    /// the number of orbs to spawn in phase 1
    /// </summary>
    public int phase1NoOfOrbs;
    /// <summary>
    /// the number of orbs to spawn in phase 2
    /// </summary>
    public int phase2NoOfOrbs;
    /// <summary>
    /// the radius of the orbs
    /// </summary>
    public int projectileRadius;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        noOfOrbs = phase1NoOfOrbs;

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            timeTillAtk = phase2StartTimeTillAtk;
            noOfOrbs = phase2NoOfOrbs;
        }

        timeBetweenAttacks = startTimeBetweenAttacks;

        count = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else 
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        if (timeBetweenAttacks > 0)
        {
            hasUsedMove = false;
        }
        else if(!hasUsedMove && count < noOfOrbs)
        {
            hasUsedMove = true;
            timeBetweenAttacks = startTimeBetweenAttacks;
            count++;

            if(isFacingRight())
            {
                GameObject projectile = Instantiate(attack, new Vector3(enemyTrans.position.x + 2, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation, enemyTrans.parent);
                projectile.transform.localScale = new Vector3(projectileRadius, projectileRadius, projectileRadius);
            }
            else
            {
                GameObject projectile = Instantiate(attack, new Vector3(enemyTrans.position.x - 2, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation, enemyTrans.parent);
                projectile.transform.localScale = new Vector3(projectileRadius, projectileRadius, projectileRadius);
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
