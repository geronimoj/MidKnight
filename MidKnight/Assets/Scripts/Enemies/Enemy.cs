using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// every enemy has this script
/// </summary>
public class Enemy : Character
{
    /// <summary>
    /// a reference to this enemy's animation
    /// </summary>
    Animator enemyAnim;
    /// <summary>
    /// a reference to this enemy's hitbox
    /// </summary>
    EnemyHitbox enemyHitbox;
    /// <summary>
    /// a reference to this enemy's collider
    /// </summary>
    Collider enemyCol;
    /// <summary>
    /// a reference to this enemy's character controller
    /// </summary>
    CharacterController enemyCC;
    /// <summary>
    /// the damage this enemy deals
    /// </summary>
    public int damage;
    /// <summary>
    /// checks if the enemy is in its second phase 
    /// </summary>
    [HideInInspector] public bool isPhase2 = false;
    /// <summary>
    /// the time until the gameobject is destroyed
    /// </summary>
    float timeTillDestroy = 5;
    /// <summary>
    /// the downward force
    /// </summary>
    public float gravity;
    /// <summary>
    /// the overall vertical speed
    /// </summary>
    public float vertSpeed;
    /// <summary>
    /// returns true if the enemy is dead
    /// </summary>
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
