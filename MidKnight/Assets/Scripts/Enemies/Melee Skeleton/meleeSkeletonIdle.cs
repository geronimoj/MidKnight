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
    Vector3 destination;
    bool isThereFloor;
    bool isThereAWall;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        skeleTrans = animator.GetComponent<Transform>();
        floorCheck = animator.GetComponentInChildren<floorCheck>();
        wallCheck = animator.GetComponentInChildren<wallCheck>();
        playerCheck = animator.GetComponentInChildren<playerCheck>();
        destination = new Vector3(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);

        //Set it's vision range in the inspector
        chaseRadius = animator.gameObject.transform.GetChild(2);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //check if floor wall or player is nearby
        isThereFloor = floorCheck.isThereFloor;
        isThereAWall = wallCheck.isThereAWall;
        bool isThereAPlayer = playerCheck.isTherePlayer;
        
        //atk as soon as skele is in range and player is above skeleton
        if(Vector3.Distance(playerTrans.position, skeleTrans.position) < atkRange && playerTrans.position.y >= skeleTrans.position.y)
        {
            animator.SetTrigger("atk");
        }

        //walk to the player if there is one, but stop if there is a wall or no floor
        if(isThereAPlayer)
        {
            FacePlayer();

            bool wallAndFloorCheck = WallAndFloorCheck();

            if (wallAndFloorCheck)
            {
                destination.Set(skeleTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
            }
            else
            {
                destination.Set(playerTrans.position.x, skeleTrans.position.y, skeleTrans.position.z);
            }
        }

        //skele always walks to destination
        skeleTrans.position = Vector3.MoveTowards(skeleTrans.position, destination, speed * Time.deltaTime);
    }

    //check for walls and floor
    bool WallAndFloorCheck()
    {
        if (isThereAWall || !isThereFloor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //face the player
    void FacePlayer()
    {
        bool playerOnRight = PlayerOnRight();

        if (playerOnRight)
        {
            skeleTrans.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            skeleTrans.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //which side is the player on
    bool PlayerOnRight()
    {
        if (playerTrans.position.x > skeleTrans.position.x)
        {
            return true;
        }
        else
        {
            return false;
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
