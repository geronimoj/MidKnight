﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocks : basePrefab
{
    /// <summary>
    /// a prefab that drops downwards
    /// </summary>
    /// 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        destination.Set(prefabTrans.position.x, -100, prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination);
    }

    /// <summary>
    /// destroy this when it hits the wall or the floor
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
