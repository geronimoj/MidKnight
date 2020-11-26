using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the skeleton king's laser storage
/// it flies into the air, expands, then shoots 5 lasers and the player then shrinks
/// </summary>
public class SkeletonKingLaserStorage : basePrefab
{
    /// <summary>
    /// a reference to the laser prefab 
    /// </summary>
    public GameObject laser;
    /// <summary>
    /// the amount of scale the sphere increases at
    /// </summary>
    public float radiusIncrease;
    /// <summary>
    /// the vector containing the radiusIncrease
    /// </summary>
    Vector3 radiusIncreaseVector;
    /// <summary>
    /// the max radius the sphere will reach
    /// </summary>
    public float maxRadius;
    /// <summary>
    /// returns true if it's spawned lsaer
    /// </summary>
    bool hasUsedMove = false;
    /// <summary>
    /// the start time between each laser
    /// </summary>
    public float startTimeBetweenAttacks;
    /// <summary>
    /// the current time to spawn the next laser
    /// </summary>
    float timeBetweenAttacks;
    /// <summary>
    /// the amount of lasers spawned so far
    /// </summary>
    int count = 0;
    /// <summary>
    /// the number of lasers to spawn
    /// </summary>
    public int noOfLasers;
    /// <summary>
    /// returns true if its reached its max radius
    /// </summary>
    bool hasReachedMaxRadius = false;
    /// <summary>
    /// the time until the prefab shrinks
    /// </summary>
    public float timeTillShrink;
    /// <summary>
    /// the z rotation of the laser
    /// </summary>
    float laserZRotation;
    /// <summary>
    /// returns true if its set its z rotation
    /// </summary>
    bool hasSetZRotation;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //initialise stuff
        radiusIncreaseVector = new Vector3(radiusIncrease, radiusIncrease, radiusIncrease);
        destination.Set(0, 20, 0);

    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination);

        //when it reaches its destination and hasnt expanded to max radius, expand
        if(prefabTrans.position == destination)
        {
            if(prefabTrans.localScale.x < maxRadius && !hasReachedMaxRadius)
            {
                prefabTrans.localScale += radiusIncreaseVector;
            }
            //if count is less than amount of lasers to fire, fire the next one and increase count
            else if (count < noOfLasers)
            {
                hasReachedMaxRadius = true;

                if (timeBetweenAttacks > 0)
                {
                    timeBetweenAttacks -= Time.deltaTime;
                    hasUsedMove = false;    
                }
                else if(!hasSetZRotation)
                {
                    //set its rotation slightly before it fires
                    hasSetZRotation = true;

                    if(playerTrans.position.x < 0)
                    {
                        laserZRotation = (-0.0542f * Mathf.Pow(playerTrans.position.x, 2)) - 3.45f * playerTrans.position.x + 269;
                    }
                    else
                    {
                        laserZRotation = -(-0.0542f * Mathf.Pow(playerTrans.position.x, 2)) - 3.45f * playerTrans.position.x + 269;
                    }
                    timeBetweenAttacks = startTimeBetweenAttacks;
                }
                else if(!hasUsedMove)
                {
                    //shoot 5 lasers
                    hasUsedMove = true;
                    hasSetZRotation = false;
                    timeBetweenAttacks = startTimeBetweenAttacks;
                    count++;

                    Instantiate(laser, prefabTrans.position, Quaternion.Euler(laserZRotation, 90, 0));
                }
            }
            else
            {
                //start shrinking
                if(timeTillShrink > 0)
                {
                    timeTillShrink -= Time.deltaTime;
                }
                else
                {
                    if (prefabTrans.localScale.x > 0)
                    {
                        prefabTrans.localScale -= radiusIncreaseVector;
                    }
                }
            }
        }
    }
}
