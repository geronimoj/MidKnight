using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseBossAttack : StateMachineBehaviour
{
    /// <summary>
    /// most boss attacks follow this formula
    /// </summary>
    
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public float timeTillAtk;
    [HideInInspector] public bool hasUsedMove;
    [HideInInspector] public Vector3 destination;
    CharacterController cc;
    public int speed;
    public float startTimeTillAtk;
    public GameObject attack;
    public float arenaUpYCoordinate;
    public float arenaDownYCoordinate;
    public float arenaLeftXCoordinate;
    public float arenaRightXCoordinate;
    public float gravity = 5;
    private float vertSpeed = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        timeTillAtk = startTimeTillAtk;
        hasUsedMove = false;
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTrans = animator.GetComponent<Transform>();
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);

        cc = animator.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("cc not found");
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

        if (gravity != 0)
        {
            dir.y = vertSpeed * Time.deltaTime;
        }
        
        cc.Move(dir);
    }

    /// <summary>
    /// move to destination at set speed
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public void MoveToDestination(Vector3 destination, int speed)
    {
        vertSpeed = -gravity;
        Vector3 dir = (destination - enemyTrans.position).normalized * speed * Time.deltaTime;

        if (gravity != 0)
        {
            dir.y = vertSpeed * Time.deltaTime;
        }

        cc.Move(dir);
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

   public bool isFacingRight()
    {
        if(enemyTrans.eulerAngles == new Vector3(0,0,0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
