using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBeam : basePrefab
{
    /// <summary>
    /// increase the size of the laser beam after x seconds
    /// </summary>
    public float timeTillSizeIncrease;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(timeTillSizeIncrease > 0)
        {
            timeTillSizeIncrease -= Time.deltaTime;
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 30);
        }
    }
}
