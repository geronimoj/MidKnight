using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemyIdle : StateMachineBehaviour
{
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Vector3 destination;
    floorCheck floorCheck;
    wallCheck wallCheck;
    CharacterController cc;
    public int speed = 1;
    public int xVisionRange;
    public int yUpVisionRange;
    public int yDownVisionRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
        floorCheck = animator.GetComponentInChildren<floorCheck>();
        wallCheck = animator.GetComponentInChildren<wallCheck>();

        cc = animator.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("cc not found");
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
    /// Check if there is a wall or no floor stopping the enemy from moving
    /// </summary>
    /// <returns></returns>
    public bool WallAndFloorCheck()
    {
        if(wallCheck == null || floorCheck == null)
        {
            Debug.LogError("wall check or floor check not found");
            return false;
        }
        else
        {
            if (wallCheck.isThereAWall || !floorCheck.isThereFloor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Check if the player is in range
    /// </summary>
    /// <returns></returns>
    public bool PlayerCheck()
    {
        if(Mathf.Abs(playerTrans.position.x - enemyTrans.position.x) < xVisionRange && playerTrans.position.y < enemyTrans.position.y + yUpVisionRange && playerTrans.position.y > enemyTrans.position.y - yDownVisionRange)
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
        cc.Move((destination - enemyTrans.position).normalized * speed * Time.deltaTime );
    }

    /// <summary>
    /// move to destination at set speed
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public void MoveToDestination(Vector3 destination, int speed)
    {
        cc.Move((destination - enemyTrans.position).normalized * speed * Time.deltaTime);
    }
}
