using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemyIdle : StateMachineBehaviour
{
    public Transform enemyTrans;
    public Transform playerTrans;
    floorCheck floorCheck;
    wallCheck wallCheck;
    playerCheck playerCheck;
    public int speed = 1;
    public Vector3 destination;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);

        floorCheck = animator.GetComponentInChildren<floorCheck>();
        wallCheck = animator.GetComponentInChildren<wallCheck>();
        playerCheck = animator.GetComponentInChildren<playerCheck>();
    }


    //face the player
    public void FacePlayer()
    {
        if (PlayerOnRight())
        {
            enemyTrans.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            enemyTrans.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //which side is the player on
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

    //check for walls and floor
    public bool WallAndFloorCheck()
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

    public bool PlayerCheck()
    {
        if(playerCheck.isTherePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveToDestination(Vector3 destination)
    {
        enemyTrans.position = Vector3.MoveTowards(enemyTrans.position, destination, speed * Time.deltaTime);
    }
}
