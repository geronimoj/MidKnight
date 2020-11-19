﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script for the ranged skeleton's magic bolt
/// </summary>
public class rangedSkeletonProjectile : basePrefab
{
    /// <summary>
    /// the speed after it reaches its first destination
    /// </summary>
    public int newSpeed;
    /// <summary>
    /// a reference to the prefab's rigidbody
    /// </summary>
    Rigidbody prefabRB;
    /// <summary>
    /// the prefab's force
    /// </summary>
    Vector3 force;
    /// <summary>
    /// returns true if it has reached its destination
    /// </summary>
    bool hasReachedDestination;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //initialise stuff
        prefabRB = GetComponent<Rigidbody>();

        //projectile flies in between the player and the enemy 
        destination.Set((playerTrans.position.x + prefabTrans.position.x) / 2, prefabTrans.position.y + 10, prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //always move to its destination

        if(prefabTrans.position != destination && !hasReachedDestination)
        {
            MoveToDestination(destination, speed);
        }
        else
        {
            hasReachedDestination = true;

            force.Set(playerTrans.position.x - prefabTrans.position.x, playerTrans.position.y - prefabTrans.position.y, playerTrans.position.z - prefabTrans.position.z);
            prefabRB.AddForce(force * newSpeed * Time.deltaTime);
        }
    }

    //destroy this when it touches a wall or the floor
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor"))
        {
            HasBeenHit();
        }
    }

    //destroy this when the player hits it
    public override void HasBeenHit()
    {
        Destroy(this.gameObject);
    }
}
