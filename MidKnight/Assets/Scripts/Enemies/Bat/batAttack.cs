using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batAttack : StateMachineBehaviour
{
    public int speed;
    Transform sleepRadius;
    public float sleepRadiusSize;
    bool playerCheck;
    Transform batTrans;
    Transform playerTrans;
    Vector3 destination;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        batTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        sleepRadius = animator.gameObject.transform.GetChild(1);
        sleepRadius.localScale = new Vector3(sleepRadiusSize, sleepRadiusSize, sleepRadiusSize);

        destination = new Vector3(batTrans.position.x, batTrans.position.y, batTrans.position.z);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCheck = animator.GetComponentInChildren<playerCheck>().isTherePlayer;

        if (playerCheck)
        {
            batTrans.position = Vector3.MoveTowards(batTrans.position, playerTrans.position, speed * Time.deltaTime);
        }
        else
        {
            batTrans.position = Vector3.MoveTowards(batTrans.position, destination, speed * Time.deltaTime);

            if (batTrans.position == destination)
            {
                animator.SetTrigger("idle");
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
