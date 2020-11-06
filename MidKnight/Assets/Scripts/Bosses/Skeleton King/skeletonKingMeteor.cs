﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonKingMeteor : basePrefab
{
    public int floorCoordinate;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        destination.Set(playerTrans.position.x, floorCoordinate, playerTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination, speed);
    }
}
