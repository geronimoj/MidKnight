using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    Animator enemyAnim;
    EnemyHitbox enemyHitbox;
    public int damage;
    [HideInInspector] public bool isPhase2 = false;
    float timeTillDestroy = 5;
    bool isDead = false;

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
        if(isDead)
        {
            timeTillDestroy -= Time.deltaTime;

            Color enemyColor = GetComponent<Renderer>().material.color;
            float fadeAmount =  enemyColor.a - (100f * Time.deltaTime);
            enemyColor = new Color(enemyColor.r, enemyColor.g, enemyColor.b, fadeAmount);
            GetComponent<Renderer>().material.color = enemyColor;
        }

        if(timeTillDestroy < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnDeath()
    {
        enemyAnim.SetTrigger("death");
        Destroy(enemyHitbox);
        isDead = true;  
    }
}
