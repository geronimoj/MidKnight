using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseBossIdle : StateMachineBehaviour
{
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public Transform playerTrans;
    int noOfMoves = 0;
    public float minStartTimeTillAtk;
    public float maxStartTimeTillAtk;
    float timeTillAtk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        timeTillAtk = Random.Range(minStartTimeTillAtk, maxStartTimeTillAtk);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else
        {
            int moveToUse = Random.Range(1, noOfMoves + 1);

            if(moveToUse == 1)
            {
                animator.SetTrigger("atk1");
            }
            if (moveToUse == 2)
            {
                animator.SetTrigger("atk2");
            }
            if (moveToUse == 3)
            {
                animator.SetTrigger("atk3");
            }
            if (moveToUse == 4)
            {
                animator.SetTrigger("atk4");
            }
            if (moveToUse == 5)
            {
                animator.SetTrigger("atk5");
            }
        }
    }
}
