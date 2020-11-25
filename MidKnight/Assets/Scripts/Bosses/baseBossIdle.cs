using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// all boss idle states derives from this
/// </summary>
public class baseBossIdle : StateMachineBehaviour
{
    /// <summary>
    /// a reference to the boss' transform
    /// </summary>
    [HideInInspector] public Transform enemyTrans;
    /// <summary>
    /// a reference to the players' transform
    /// </summary>
    [HideInInspector] public Transform playerTrans;
    /// <summary>
    /// the boss' destination
    /// </summary>
    [HideInInspector] public Vector3 destination;
    /// <summary>
    /// a reference to the enemy script
    /// </summary>
    [HideInInspector] public Enemy enemy;
    /// <summary>
    /// the move which the boss will move
    /// </summary>
    [HideInInspector] public int moveToUse;
    /// <summary>
    /// the second last move the boss used
    /// </summary>
    [HideInInspector] public int secondLastMove = 0;
    /// <summary>
    /// the last move the boss used
    /// </summary>
    [HideInInspector] public int lastMove = 0;
    /// <summary>
    /// the time till the boss uses its move
    /// </summary>
    [HideInInspector] public float timeTillAtk;
    /// <summary>
    /// a reference to the boss' character controller
    /// </summary>
    [HideInInspector] public CharacterController cc;
    /// <summary>
    /// A reference to the game manager
    /// </summary>
    GameManager gm;
    /// <summary>
    /// the number of moves that the boss has
    /// </summary>
    [Range(1,7)] public int noOfMoves = 5;
    /// <summary>
    /// the minimum time till the boss attacks
    /// </summary>
    public float minStartTimeTillAtk;
    /// <summary>
    /// the maximum time till the boss attacks
    /// </summary>
    public float maxStartTimeTillAtk;
    /// <summary>
    /// the boss' speed
    /// </summary>
    public int speed;
    /// <summary>
    /// the boss' gravity
    /// </summary>
    public float gravity = 5;
    /// <summary>
    /// the boss' vertical speed
    /// </summary>
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

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

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
        //Get the direction to look along the path
        Vector3 dir = gm.GetPathDirectionRight(enemyTrans.position);
        //Rotate dir 90 degrees and use LookRotation to turn it into a quaternion
        if (dir != Vector3.zero)
            enemyTrans.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    /// <summary>
    /// makes the enemy turn to the left
    /// </summary>
    public void FaceLeft()
    {
        //Get the direction to look along the path
        Vector3 dir = -gm.GetPathDirectionRight(enemyTrans.position);
        //Rotate dir 90 degrees and use LookRotation to turn it into a quaternion
        if (dir != Vector3.zero)
            enemyTrans.rotation = Quaternion.LookRotation(dir, Vector3.up);
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
        dir = gm.MoveAlongPath(enemyTrans.position, dir);

        cc.Move(dir);
    }
}
