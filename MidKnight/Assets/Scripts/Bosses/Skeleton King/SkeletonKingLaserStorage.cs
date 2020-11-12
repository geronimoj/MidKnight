using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingLaserStorage : basePrefab
{
    /// <summary>
    /// the skeleton king's laser storage
    /// it flies into the air, expands, then shoots 5 lasers and the player then shrinks
    /// </summary>
    public GameObject laser;
    public float radiusIncrease;
    Vector3 radiusIncreaseVector;
    public int maxRadius;
    bool hasUsedMove = false;
    public float startTimeBetweenAttacks;
    float timeBetweenAttacks;
    int count = 0;
    public int noOfLasers;
    bool hasReachedMaxRadius = false;
    public float timeTillShrink;
    float laserZRotation;
    bool hasSetZRotation;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //initialise stuff
        radiusIncreaseVector = new Vector3(radiusIncrease, radiusIncrease, radiusIncrease);
        destination.Set(0, 13, 0);

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
                        laserZRotation = (0.096f * Mathf.Pow(playerTrans.position.x, 2)) + 4.78f * playerTrans.position.x + 269.945f;
                    }
                    else
                    {
                        laserZRotation = -(0.096f * Mathf.Pow(playerTrans.position.x, 2)) + 4.78f * playerTrans.position.x + 269.945f;
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

                    Instantiate(laser, prefabTrans.position, Quaternion.Euler(0, 0, laserZRotation));
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
