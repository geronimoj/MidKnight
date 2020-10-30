using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingRatIdle : baseBossIdle
{
    bool hasUsedBossMove1;
    bool hasUsedBossMove2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //use these moves when they hit this amount of health
        if (enemy.Health * 2 / 3 < enemy.MaxHealth && !hasUsedBossMove1)
        {
            hasUsedBossMove1 = true;
        }
        else if (enemy.Health / 3 < enemy.MaxHealth && !hasUsedBossMove2)
        {
            moveToUse = 5;
            hasUsedBossMove2 = true;
        }

        moveToUse = 1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        destination.Set(playerTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
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
