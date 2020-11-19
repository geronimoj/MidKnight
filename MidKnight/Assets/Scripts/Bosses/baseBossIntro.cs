using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base boss intro script
/// </summary>
public class baseBossIntro : StateMachineBehaviour
{
    /// <summary>
    /// the vision range of the boss
    /// </summary>
    public float distanceFromPlayerToWake;
    /// <summary>
    /// a reference to the player's transform
    /// </summary>
    Transform playerTrans;
    /// <summary>
    /// a reference to the boss' transform
    /// </summary>
    Transform enemyTrans;
    /// <summary>
    /// returns true if the boss is awake
    /// </summary>
    bool isAwake = false;
    /// <summary>
    /// the time till the boss changes to idle animation
    /// </summary>
    public float timeTillIdle;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTrans = animator.GetComponent<Transform>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector3.Distance(playerTrans.position, enemyTrans.position) < distanceFromPlayerToWake)
        {
            isAwake = true;
        }

        if(isAwake && timeTillIdle > 0)
        {
            timeTillIdle -= Time.deltaTime;
        }
        else if(timeTillIdle < 0)
        {
            animator.SetTrigger("idle");
        }
    }
}
