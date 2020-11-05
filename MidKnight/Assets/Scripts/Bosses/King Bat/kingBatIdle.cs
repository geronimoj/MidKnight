using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingBatIdle : baseBossIdle
{
    public float arenaUpYCoordinate;
    public float arenaDownYCoordinate;
    public float arenaLeftXCoordinate;
    public float arenaRightXCoordinate;
    public int minDistFromDestination;
    bool hasUsedBossMove1;
    bool hasUsedBossMove2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        while (Vector3.Distance(destination, enemyTrans.position) < minDistFromDestination)
        {
            destination.Set(Random.Range(arenaLeftXCoordinate, arenaRightXCoordinate), Random.Range(arenaDownYCoordinate, arenaUpYCoordinate), enemyTrans.position.z);
        }

        //use these moves when they hit this amount of health
        if (enemy.Health < enemy.MaxHealth * 2 / 3 && !hasUsedBossMove1)
        {
            moveToUse = 5;
            hasUsedBossMove1 = true;
        }
        else if (enemy.Health < enemy.MaxHealth / 3 && !hasUsedBossMove2)
        {
            moveToUse = 5;
            hasUsedBossMove2 = true;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

        if(destination.x > enemyTrans.position.x)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }

        if (Vector3.Distance(destination, enemyTrans.position) < 0.1f)
        {
            Debug.Log("King Bat Reached Destination");

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
