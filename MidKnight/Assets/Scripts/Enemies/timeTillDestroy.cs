using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// destroy the object after this timer
/// </summary>
public class timeTillDestroy : MonoBehaviour
{
    /// <summary>
    /// count until the object is destroyed
    /// </summary>
    public float startTimeTillDestroy;
    // Update is called once per frame
    void Update()
    {
        if(startTimeTillDestroy > 0)
        {
            startTimeTillDestroy -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
