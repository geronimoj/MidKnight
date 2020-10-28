using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeBatSonicWave : basePrefab
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if(playerTrans.position.x - prefabTrans.position.x == 0 && playerTrans.position.y - prefabTrans.position.y == 0)
        {
            destination.Set(0, 10000, 0);
        }
        else
        {
            destination.Set(10000 * (playerTrans.position.x - prefabTrans.position.x), 10000 * (playerTrans.position.y - prefabTrans.position.y), prefabTrans.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination);
    }
}
