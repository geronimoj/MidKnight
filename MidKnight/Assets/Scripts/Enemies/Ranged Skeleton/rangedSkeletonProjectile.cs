using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedSkeletonProjectile : basePrefab
{
    /// <summary>
    /// script for the ranged skeleton's magic bolt
    /// </summary>

    public int newSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //projectile flies in between the player and the enemy 
        destination.Set((playerTrans.position.x + prefabTrans.position.x) / 2, prefabTrans.position.y + 5, prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //always move to its destination
        MoveToDestination(destination, speed);

        //if it reaches its first destination fly at the player
        if(prefabTrans.position == destination)
        {
            destination.Set(10000 * (playerTrans.position.x - prefabTrans.position.x), 10000 * (playerTrans.position.y - prefabTrans.position.y), prefabTrans.position.z);
            speed = newSpeed;
        }
    }

    //destroy this when it touches a wall or the floor
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }

}
