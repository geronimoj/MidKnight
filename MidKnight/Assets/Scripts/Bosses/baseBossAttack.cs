﻿using System.Collections;
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
    public int speed;
    public float startTimeTillAtk;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialise stuff
        timeTillAtk = startTimeTillAtk;
        hasUsedMove = false;

        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTrans = animator.GetComponent<Transform>();
    }

}