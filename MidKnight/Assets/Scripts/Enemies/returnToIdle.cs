using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Return to idle after x seconds
/// </summary>
public class returnToIdle : StateMachineBehaviour
{
    /// <summary>
    /// the minimum start time it takes for the enemy to return to idle
    /// </summary>
    public float minStartTimeTillIdle;
    /// <summary>
    /// the maximum start time it takes for the enemy to return to idle
    /// </summary>
    public float maxStartTimeTillIdle;
    /// <summary>
    /// a random number taken from the min and max
    /// </summary>
    float timeTillIdle;
    /// <summary>
    /// the minimum start time it takes for the enemy to return to idle if it's in phase 2
    /// </summary>
    public float phase2MinStartTimeTillIdle;
    /// <summary>
    /// the maximum start time it takes for the enemy to return to idle if it's in phase 2
    /// </summary>
    public float phase2MaxStartTimeTillIdle;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeTillIdle = Random.Range(minStartTimeTillIdle, maxStartTimeTillIdle);

        if(animator.GetComponent<Enemy>().isPhase2)
        {
            timeTillIdle = Random.Range(phase2MinStartTimeTillIdle, phase2MaxStartTimeTillIdle);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timeTillIdle > 0)
        {
            timeTillIdle -= Time.deltaTime;
        }
        else
        {
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
