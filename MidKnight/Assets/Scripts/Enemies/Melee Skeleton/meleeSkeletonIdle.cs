using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSkeletonIdle : StateMachineBehaviour
{
    /// <summary>
    /// Melee AND ranged skeleton idle 
    /// </summary>
    
    public float minStartTimeTillMove;
    public float maxStartTimeTillMove;
    Transform skeleTrans;
    public int speed;
    Transform chaseRadius;
    public float chaseRadiusSize;
    Transform playerTrans;
    public int atkRange;

    floorCheck floorCheck;
    wallCheck wallCheck;
    playerCheck playerCheck;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        skeleTrans = animator.GetComponent<Transform>();

        floorCheck = animator.GetComponentInChildren<floorCheck>();
        wallCheck = animator.GetComponentInChildren<wallCheck>();
        playerCheck = animator.GetComponentInChildren<playerCheck>();

        //Set it's vision range in the inspector
        chaseRadius = animator.gameObject.transform.GetChild(2);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //check if floor wall or player is nearby
        bool isThereFloor = floorCheck.isThereFloor;
        bool isThereAWall = wallCheck.isThereAWall;
        bool isThereAPlayer = playerCheck.isTherePlayer;
        
        if(Mathf.Abs(playerTrans.position.x - skeleTrans.position.x) < atkRange)
        {
            animator.SetTrigger("atk");
        }

        if(isThereAPlayer)
        {
            skeleTrans.position = Vector3.MoveTowards(skeleTrans.position, playerTrans.position, speed * Time.deltaTime);
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
