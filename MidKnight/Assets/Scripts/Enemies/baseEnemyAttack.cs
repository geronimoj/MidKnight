﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All basic enemies attack animations will derive from this
/// </summary>
public class baseEnemyAttack : StateMachineBehaviour
{
    /// <summary>
    /// the start time the enemy takes until it uses its attack
    /// </summary>
    public float startTimeTillAtk;
    /// <summary>
    /// the current time the enemy takes until it uses its attack
    /// </summary>
    [HideInInspector] public float timeTillAtk;
    /// <summary>
    /// returns false until the enemy uses its attack. ensures enemies will only use their attack once
    /// </summary>
    [HideInInspector] public bool hasUsedAtk;
    /// <summary>
    /// a reference to the enemy's transform
    /// </summary>
    [HideInInspector] public Transform enemyTrans;
    /// <summary>
    /// a reference to the player's trasnform
    /// </summary>
    [HideInInspector] public Transform playerTrans;
    /// <summary>
    /// the enemy's destination
    /// </summary>
    [HideInInspector] public Vector3 destination;
    /// <summary>
    /// a reference to the character controller
    /// </summary>
    CharacterController cc;
    /// <summary>
    /// A reference to the game manager
    /// </summary>
    GameManager gm;
    /// <summary>
    /// the enemy's speed
    /// </summary>
    public float speed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeTillAtk = startTimeTillAtk;
        hasUsedAtk = false;
        enemyTrans = animator.GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        destination = new Vector3(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);

        cc = animator.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("cc not found");
        }
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        destination.Set(enemyTrans.position.x, enemyTrans.position.y, enemyTrans.position.z);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveToDestination(destination);

    }

    /// <summary>
    /// Make the enemy move to this destination
    /// </summary>
    /// <param name="destination"></param>
    public virtual void MoveToDestination(Vector3 destination)
    {
        Enemy e = enemyTrans.GetComponent<Enemy>();
        e.vertSpeed = -e.gravity;
        Vector3 dir = (destination - enemyTrans.position).normalized * speed * Time.deltaTime;

        if (e.gravity != 0)
        {
            dir.y = e.vertSpeed * Time.deltaTime;
        }

        if (e.BeingKnockedBack)
        {
            dir = e.knockBackDir * e.knockBackForce * Time.deltaTime;
        }
        dir = gm.MoveAlongPath(enemyTrans.position, dir);

        cc.Move(dir);
    }

    /// <summary>
    /// move to destination at set speed
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public void MoveToDestination(Vector3 destination, int speed)
    {
        Enemy e = enemyTrans.GetComponent<Enemy>();
        e.vertSpeed = -e.gravity;
        Vector3 dir = (destination - enemyTrans.position).normalized * speed * Time.deltaTime;

        if (e.gravity != 0)
        {
            dir.y = e.vertSpeed * Time.deltaTime;
        }

        if (e.BeingKnockedBack)
        {
            dir = e.knockBackDir * e.knockBackForce * Time.deltaTime;
        }
        dir = gm.MoveAlongPath(enemyTrans.position, dir);

        cc.Move(dir);
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
}
