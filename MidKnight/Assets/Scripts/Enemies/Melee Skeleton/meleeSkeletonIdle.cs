using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeSkeletonIdle : baseEnemyIdle
{
    /// <summary>
    /// Melee AND ranged skeleton idle 
    /// </summary>
    
    public int atkRange;
    Transform chaseRadius;
    public float chaseRadiusSize;
    public float minStartTimeTillWalk;
    public float maxStartTimeTillWalk;
    float timeTillWalk;
    bool hasChosenWalk;
    public float maxDistToWalk;
    float distToWalk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);


        //initialise stuff
        hasChosenWalk = false;

        //custom stuff for skele
        //atk as soon as skele is in range and player is above skeleton
        AttackPlayer(animator);

        //change the radius of skele vision in inspector
        chaseRadius = animator.gameObject.transform.GetChild(2);
        chaseRadius.localScale = new Vector3(chaseRadiusSize, chaseRadiusSize, chaseRadiusSize);
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
        if (Vector3.Distance(playerTrans.position, enemyTrans.position) < atkRange)
        {
            animator.SetTrigger("atk");
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
