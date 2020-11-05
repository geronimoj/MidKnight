using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class werewolfIdle : baseBossIdle
{
    public int arenaLeftXCoordinate;
    public int ArenaRightXCoordinate;
    public int phase2speed;
    bool hasUsedBossMove = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
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

        if (Vector3.Distance(destination, enemyTrans.position) < 0.3f)
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
