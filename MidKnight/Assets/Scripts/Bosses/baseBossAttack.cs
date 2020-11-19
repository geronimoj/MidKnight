using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// all boss attack states derives from this
/// </summary>
public class baseBossAttack : StateMachineBehaviour
{

    /// <summary>
    /// a reference to the players' transform
    /// </summary>
    [HideInInspector] public Transform playerTrans;
    /// <summary>
    /// a reference to the boss' transform
    /// </summary>
    [HideInInspector] public Transform enemyTrans;
    /// <summary>
    /// the time till the boss uses its move
    /// </summary>
    [HideInInspector] public float timeTillAtk;
    /// <summary>
    /// returns true if the boss has used its move
    /// </summary>
    [HideInInspector] public bool hasUsedMove;
    /// <summary>
    /// the boss' destination
    /// </summary>
    [HideInInspector] public Vector3 destination;
    /// <summary>
    /// the boss' character controller
    /// </summary>
    CharacterController cc;
    /// <summary>
    /// the boss' speed
    /// </summary>
    public int speed;
    /// <summary>
    /// the start time till the boss uses its move
    /// </summary>
    public float startTimeTillAtk;
    /// <summary>
    /// the prefab the boss will instantiate
    /// </summary>
    public GameObject attack;
    /// <summary>
    /// the up y arena coordinate that the boss is in
    /// </summary>
    public float arenaUpYCoordinate;
    /// <summary>
    /// the down y arena coordinate that the boss is in
    /// </summary>
    public float arenaDownYCoordinate;
    /// <summary>
    /// the left x arena coordinate that the boss is in
    /// </summary>
    public float arenaLeftXCoordinate;
    /// <summary>
    /// the right x arena coordinate that the boss is in
    /// </summary>
    public float arenaRightXCoordinate;
    /// <summary>
    /// the boss' gravity
    /// </summary>
    public float gravity = 5;
    /// <summary>
    /// the boss vertical speed
    /// </summary>
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

    /// <summary>
    /// check if the enemy is facing the right
    /// </summary>
    /// <returns></returns>
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
