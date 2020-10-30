using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeBatSonicWave : basePrefab
{
    /// <summary>
    /// the script for the bat's sonic wave
    /// </summary>

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        destination.Set(1000 * (playerTrans.position.x - prefabTrans.position.x), 1000 * (playerTrans.position.y - prefabTrans.position.y), prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //fly to the destination
        MoveToDestination(destination);
    }
}
