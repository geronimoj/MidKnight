using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Melee AND ranged skeleton idle 
/// </summary>
public class meleeSkeletonIdle : baseEnemyIdle
{
    /// <summary>
    /// the skeleton's attack range
    /// </summary>
    public int atkRange;
    /// <summary>
    /// the minimum start time it takes for the skeleton to walk
    /// </summary>
    public float minStartTimeTillWalk;
    /// <summary>
    /// the maximum start time it takes for the skeleton to walk
    /// </summary>
    public float maxStartTimeTillWalk;
    /// <summary>
    /// a random time taken from the minimum and maximum for the skeleton to walk
    /// </summary>
    float timeTillWalk;
    /// <summary>
    /// returns true if he has chosen to walk
    /// </summary>
    bool hasChosenWalk;
    /// <summary>
    /// the maximum distance for the skeleton to walk
    /// </summary>
    public float maxDistToWalk;
    /// <summary>
    /// a random distance for the skeleton to walk, taken from the maximum distance
    /// </summary>
    float distToWalk;
    /// <summary>
    /// the skeleton chooses which use to move
    /// </summary>
    int moveToUse;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //initialise stuff
        hasChosenWalk = false;

        //custom stuff for skele
        //atk as soon as skele is in range and player is above skeleton
        AttackPlayer(animator);

        //face the player
        FacePlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //atk as soon as skele is in range 
        AttackPlayer(animator);

        //walk to the player if there is one, but stop if there is a wall or no floor
        if(PlayerCheck())
        {
            FacePlayer();

            destination.Set(playerTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);

            if (WallAndFloorCheck())
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }
            else
            {
                destination.Set(playerTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }

            MoveToDestination(destination);
        }
        //if the player isnt in vision, walk randomly
        else if(!hasChosenWalk)
        {
            timeTillWalk = Random.Range(minStartTimeTillWalk, maxStartTimeTillWalk);
            distToWalk = Random.Range(-maxDistToWalk, maxDistToWalk);
            destination.Set(enemyTrans.position.x + distToWalk, enemyTrans.position.y, enemyTrans.position.z);
            hasChosenWalk = true;
        }
        else if(timeTillWalk > 0)
        {
            timeTillWalk -= Time.deltaTime;
        }
        else
        {
            MoveToDestination(destination);

            //face the way its walking
            if(distToWalk > 0)
            {
                FaceRight();
            }
            else
            {
                FaceLeft();
            }

            //stop moving if theres a wall or no floor
            if (WallAndFloorCheck())
            {
                destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
            }

            //choose somewhere else to walk after reaching its destination
            if (enemyTrans.position == destination)
            {
                hasChosenWalk = false;
            }
        }
    }

    /// <summary>
    /// attack the player if skele is in range
    /// </summary>
    /// <param name="animator"></param>
    void AttackPlayer(Animator animator)
    {
        //do extra stuff on the ranged skeletons
        if (animator.name == "Ranged Skeleton")
        {
            moveToUse = Random.Range(1, 3);
        }
        else
        {
            moveToUse = 1;
        }

        if (Vector3.Distance(playerTrans.position, enemyTrans.position) < atkRange)
        {
            if (moveToUse == 1)
            {
                animator.SetTrigger("atk");
            }
            else
            {
                animator.SetTrigger("atk2");
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
