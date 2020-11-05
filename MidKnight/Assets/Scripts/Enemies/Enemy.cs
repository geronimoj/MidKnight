using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    Animator enemyAnim;
    EnemyHitbox enemyHitbox;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        //initialise stuff
        enemyAnim = GetComponent<Animator>();
        enemyHitbox = GetComponentInChildren<EnemyHitbox>();
        health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
        enemyAnim.SetTrigger("Death");
        Destroy(enemyHitbox);
    }
}
