using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemyAttack : StateMachineBehaviour
{
    public float startTimeTillAtk;
    [HideInInspector] public float timeTillAtk;
    [HideInInspector] public bool hasUsedAtk;
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Vector3 destination;
    public float speed;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeTillAtk = startTimeTillAtk;
        hasUsedAtk = false;
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
    }

    /// <summary>
    /// Make the enemy move to this destination
    /// </summary>
    /// <param name="destination"></param>
    public void MoveToDestination(Vector3 destination)
    {
        enemyTrans.position = Vector3.MoveTowards(enemyTrans.position, destination, speed * Time.deltaTime);
    }

    /// <summary>
    /// move to destination at set speed
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public void MoveToDestination(Vector3 destination, int speed)
    {
        enemyTrans.position = Vector3.MoveTowards(enemyTrans.position, destination, speed * Time.deltaTime);
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
}
