using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batAttack : StateMachineBehaviour
{
    /// <summary>
    /// the bat's attack
    /// </summary>
    
    public int speed;
    Transform sleepRadius;
    public float sleepRadiusSize;
    playerCheck playerCheck;
    Transform batTrans;
    Transform playerTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        batTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerCheck = animator.GetComponentInChildren<playerCheck>();

        //Change the radius for the bat to sleep in the inspector
        sleepRadius = animator.gameObject.transform.GetChild(0);
        sleepRadius.localScale = new Vector3(sleepRadiusSize, sleepRadiusSize, sleepRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if the player is nearby
        bool isThereAPlayer = playerCheck.isTherePlayer;

        //if the player is nearby, chase the player
        if (isThereAPlayer)
        {
            //Always move to destination
            batTrans.position = Vector3.MoveTowards(batTrans.position, playerTrans.position, speed * Time.deltaTime);

            FacePlayer();
        }
    }

    //face the player
    void FacePlayer()
    {
        bool playerOnRight = PlayerOnRight();

        if (playerOnRight)
        {
            batTrans.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            batTrans.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //which side is the player on
    bool PlayerOnRight()
    {
        if (playerTrans.position.x > batTrans.position.x)
        {
            return true;
        }
        else
        {
            return false;
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
