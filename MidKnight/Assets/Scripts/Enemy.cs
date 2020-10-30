using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    Animator enemyAnim;
    GameObject hitBox;

    // Start is called before the first frame update
    void Start()
    {
        //initialise stuff

        enemyAnim = GetComponent<Animator>();
        //hitBox = GetComponentInChildren<>();
        health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
        enemyAnim.SetTrigger("Death");
        Destroy(hitBox);
    }


}
