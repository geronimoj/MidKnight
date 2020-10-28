using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedSkeletonProjectile : basePrefab
{
    public int newSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        destination.Set((playerTrans.position.x + prefabTrans.position.x) / 2, prefabTrans.position.y + 5, prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination, speed);

        if(prefabTrans.position == destination)
        {
            destination.Set(10000 * (playerTrans.position.x - prefabTrans.position.x), 10000 * (playerTrans.position.y - prefabTrans.position.y), prefabTrans.position.z);
            speed = newSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boundary")
        {
            Destroy(this);
        }
    }
}
