﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    /// <summary>
    /// every enemy has this script
    /// </summary>
    Animator enemyAnim;
    EnemyHitbox enemyHitbox;
    Collider enemyCol;
    CharacterController enemyCC;
    public int damage;
    [HideInInspector] public bool isPhase2 = false;
    float timeTillDestroy = 5;
    public float gravity;
    public float vertSpeed;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //initialise stuff
        enemyAnim = GetComponent<Animator>();
        enemyHitbox = GetComponentInChildren<EnemyHitbox>();
        enemyCC = GetComponent<CharacterController>();
        health = MaxHealth;
    }

    // Update is called once per frame
    protected override void ExtraUpdate()
    {
        if(isDead)
        {
            timeTillDestroy -= Time.deltaTime;
        }

        if(timeTillDestroy < 0)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Call this when the enemy dies
    /// </summary>
    public override void OnDeath()
    {
        enemyAnim.SetTrigger("death");
        enemyCC.enabled = false;
        Destroy(enemyHitbox);
        isDead = true;  
    }
}
