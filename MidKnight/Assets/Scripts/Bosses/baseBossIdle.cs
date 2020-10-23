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
    int moveToUse;
    int secondLastMove = 0;
    int lastMove = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timeTillAtk = Random.Range(minStartTimeTillAtk, maxStartTimeTillAtk);
        moveToUse = Random.Range(1, noOfMoves + 1);

        //ensures no boss will use the same move three times in a row
        while(moveToUse == lastMove && moveToUse == secondLastMove)
        {
            moveToUse = Random.Range(1, noOfMoves + 1);
            lastMove = moveToUse;
            lastMove = secondLastMove;
        }

        lastMove = moveToUse;
        secondLastMove = lastMove;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //counter till its time to attack
        if (timeTillAtk > 0)
        {
            timeTillAtk -= Time.deltaTime;
        }
        else
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
}
