using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseBossIdle : StateMachineBehaviour
{
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public int moveToUse;
    [HideInInspector] public int secondLastMove = 0;
    [HideInInspector] public int lastMove = 0;
    [HideInInspector] public float timeTillAtk;
    [HideInInspector] public CharacterController cc;
    [Range(1,7)] public int noOfMoves = 5;
    public float minStartTimeTillAtk;
    public float maxStartTimeTillAtk;
    public int speed;
    public float gravity = 5;
    private float vertSpeed = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = animator.GetComponent<Enemy>();
        timeTillAtk = Random.Range(minStartTimeTillAtk, maxStartTimeTillAtk);
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
        cc = animator.GetComponent<CharacterController>();

        moveToUse = Random.Range(1, noOfMoves + 1);
        //ensures no boss will use the same move three times in a row
        while (moveToUse == lastMove && moveToUse == secondLastMove)
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
        //face the player 
        FacePlayer();

        //move to its destination
        MoveToDestination(destination);

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
                case 6:
                    animator.SetTrigger("atk6");
                    break;
                case 7:
                    animator.SetTrigger("atk7");
                    break;
            }
        }
    }

    /// <summary>
    /// Make the enemy face the player
    /// </summary>
    public void FacePlayer()
    {
        if (PlayerOnRight())
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    /// <summary>
    /// Makes the enemy turn to the right
    /// </summary>
    public void FaceRight()
    {
        enemyTrans.eulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// makes the enemy turn to the left
    /// </summary>
    public void FaceLeft()
    {
        enemyTrans.eulerAngles = new Vector3(0, 180, 0);
    }

    /// <summary>
    ///check which side of the enemy the player is on
    /// </summary>
    /// <returns></returns>
    public bool PlayerOnRight()
    {
        if (playerTrans.position.x > enemyTrans.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Make the enemy move to this destination
    /// </summary>
    /// <param name="destination"></param>
    public void MoveToDestination(Vector3 destination)
    {
        vertSpeed = -gravity;
        Vector3 dir = (destination - enemyTrans.position).normalized * speed * Time.deltaTime;
        
        if(gravity != 0)
        {
            dir.y = vertSpeed * Time.deltaTime;
        }

        cc.Move(dir);
    }
}
