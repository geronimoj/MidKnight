using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseBossIntro : StateMachineBehaviour
{
    /// <summary>
    /// Base boss intro script
    /// </summary>
    public float distanceFromPlayerToWake;
    Transform playerTrans;
    Transform enemyTrans;
    bool isAwake = false;
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
