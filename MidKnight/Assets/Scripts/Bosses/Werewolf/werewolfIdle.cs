using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// werewolf's idle state.
/// it runs to one side of the arena then uses its move
/// </summary>
public class werewolfIdle : baseBossIdle
{
    /// <summary>
    /// the left x coordinate of the arena
    /// </summary>
    public int arenaLeftXCoordinate;
    /// <summary>
    /// the right x coordinate of the arena
    /// </summary>
    public int ArenaRightXCoordinate;
    /// <summary>
    /// the speed in phase 2
    /// </summary>
    public int phase2speed;
    /// <summary>
    /// returns true if its used its boss move 
    /// </summary>
    bool hasUsedBossMove = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        //face away from the player and run to that side of the arena
        if(playerTrans.position.x > enemyTrans.position.x)
        {
            FaceLeft();
            destination.Set(arenaLeftXCoordinate, enemyTrans.position.y, enemyTrans.position.z);
        }
        else
        {
            FaceRight();
            destination.Set(ArenaRightXCoordinate, enemyTrans.position.y, enemyTrans.position.z);
        }

        //when it has under half health it switches to phase 2
        if (!hasUsedBossMove && enemy.Health <= 1 / 2 * enemy.MaxHealth)
        {
            moveToUse = 5;
            hasUsedBossMove = true;
        }

        if (animator.GetComponent<Enemy>().isPhase2)
        {
            speed = phase2speed;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

        //use its move at the end of the arena
        if (Vector3.Distance(destination, enemyTrans.position) < 0.2f)
        {
            switch (moveToUse)
            {
                case 1:
                    animator.SetTrigger("atk1");
                    break;

                case 2:
                    animator.SetTrigger("atk2");
                    break;

                case 3:
                    animator.SetTrigger("atk3");
                    break;

                case 4:
                    animator.SetTrigger("atk4");
                    break;

                case 5:
                    animator.SetTrigger("atk5");
                    break;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FacePlayer();   
    }

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
