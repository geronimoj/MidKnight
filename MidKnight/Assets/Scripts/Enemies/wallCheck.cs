﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if this object is colliding with a wall 
/// </summary>
public class wallCheck : MonoBehaviour
{
    /// <summary>
    /// returns true if there is a wall colliding with this game object
    /// </summary>
    public bool isThereAWall;

    // Start is called before the first frame update
    void Start()
    {
        isThereAWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            isThereAWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isThereAWall = false;
        }
    }
}
