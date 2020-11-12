using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingBatAttack3 : baseBossAttack
{
    /// <summary>
    /// king bat's third attack
    /// it fires a sonic wave at the player
    /// </summary>
    public int sonicWaveSpeed;
    public float sonicWaveSize;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);    
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

            GameObject atk = Instantiate(attack, enemyTrans.position, enemyTrans.rotation);
            atk.GetComponent<largeBatSonicWave>().speed = sonicWaveSpeed;
            atk.GetComponent<Transform>().localScale = new Vector3(sonicWaveSize, sonicWaveSize, sonicWaveSize);
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
