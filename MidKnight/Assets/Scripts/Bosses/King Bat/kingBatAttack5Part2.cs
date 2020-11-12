using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingBatAttack5Part2 : baseBossAttack
{
    /// <summary>
    /// king bat's fifth attack part 2
    /// it shoots a sonic wave at the player and drops rocks
    /// </summary>
    public int sonicWaveSpeed;
    public float sonicWaveSize;
    public float startTimeTillAtk5;
    float timeTillAtk5;
    public int noOfRocks;
    Vector3 spawnPos;
    public GameObject rocks;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        timeTillAtk5 = startTimeTillAtk5;
        spawnPos = new Vector3(0, 0, 0);

        for (int i = 0; i < noOfRocks; i++)
        {
            spawnPos.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaUpYCoordinate + 5, arenaUpYCoordinate + 20), enemyTrans.position.z);
            Instantiate(rocks, spawnPos, enemyTrans.rotation);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else if (!hasUsedMove)
        {
            hasUsedMove = true;
            GameObject atk = Instantiate(attack, enemyTrans.position, enemyTrans.rotation);
            atk.GetComponent<largeBatSonicWave>().speed = sonicWaveSpeed;
            atk.GetComponent<Transform>().localScale = new Vector3(sonicWaveSize, sonicWaveSize, sonicWaveSize);
        }

        if(timeTillAtk5 > 0)
        {
            timeTillAtk5 -= Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("atk5");
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
