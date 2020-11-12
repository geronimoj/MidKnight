using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonKingAtk2 : baseBossAttack
{
    /// <summary>
    /// the skeleton king's first attack
    /// it summons homing orbs
    /// </summary>
    public float phase2StartTimeTillAtk;
    float timeBetweenAttacks;
    public float startTimeBetweenAttacks;
    int noOfOrbs;
    int count;
    public int phase1NoOfOrbs;
    public int phase2NoOfOrbs;
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
                GameObject projectile = Instantiate(attack, new Vector3(enemyTrans.position.x + 2, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation);
                projectile.transform.localScale = new Vector3(projectileRadius, projectileRadius, projectileRadius);
            }
            else
            {
                GameObject projectile = Instantiate(attack, new Vector3(enemyTrans.position.x - 2, enemyTrans.position.y + 2, enemyTrans.position.z), enemyTrans.rotation);
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
