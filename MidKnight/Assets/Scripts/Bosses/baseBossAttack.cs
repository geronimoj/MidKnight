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
    CharacterController cc;
    public int speed;
    public float startTimeTillAtk;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        timeTillAtk = startTimeTillAtk;
        hasUsedMove = false;
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTrans = animator.GetComponent<Transform>();
        
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
        cc.Move((destination - enemyTrans.position).normalized * speed * Time.deltaTime);
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
