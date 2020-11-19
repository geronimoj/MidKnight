using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the script for the enemy rush
/// </summary>
public class EnemyRush : MonoBehaviour
{
    /// <summary>
    /// a reference to the rat
    /// </summary>
    public GameObject rat;
    /// <summary>
    /// a reference to the large rat
    /// </summary>
    public GameObject largeRat;
    /// <summary>
    /// a reference to the bat
    /// </summary>
    public GameObject bat;
    /// <summary>
    /// a reference to the large bat
    /// </summary>
    public GameObject largeBat;
    /// <summary>
    /// a reference to the melee skeleton
    /// </summary>
    public GameObject meleeSkeleton;
    /// <summary>
    /// a reference to the ranged skeleton
    /// </summary>
    public GameObject rangedSkeleton;
    /// <summary>
    /// the arena's left x coordinate
    /// </summary>
    public float ArenaLeftXCoordinate;
    /// <summary>
    /// the arena's right x coordinate
    /// </summary>
    public float ArenaRightXCoordinate;
    /// <summary>
    /// the arena's up y coordinate
    /// </summary>
    public float ArenaUpYCoordinate;
    /// <summary>
    /// the spawn point of the enemy
    /// </summary>
    Vector3 spawnPos;
    /// <summary>
    /// returns true if the enemy rush is in progress
    /// </summary>
    bool startEnemyRush = false;
    /// <summary>
    /// a reference to the player's transform
    /// </summary>
    Transform playerTrans;
    /// <summary>
    /// the list of enemies in the enemy rush
    /// </summary>
    List<GameObject> Enemies;
    int i = 0;
    /// <summary>
    /// the array of enemies in the room
    /// </summary>
    GameObject[] enemiesArray;
    /// <summary>
    /// the number of enemies in the room
    /// </summary>
    int noOfEnemies;
    /// <summary>
    /// returns true if the player has completed this
    /// </summary>
    public bool hasCompletedEnemyRush = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //initialise stuff
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnPos = new Vector3(0, 0, 0);

        //the list of enemies to fight
        Enemies = new List<GameObject>()
        {
            rat,
            rat,
            largeRat,
            rat,
            largeRat, 
            largeRat,
            bat,
            bat,
            largeBat,
            largeRat,
            largeBat,
            bat,
            rat,
            meleeSkeleton,
            largeRat,
            meleeSkeleton,
            largeRat,
            rangedSkeleton,
            meleeSkeleton,
            meleeSkeleton,
            largeBat,
            rangedSkeleton,
            rangedSkeleton,
            meleeSkeleton,
            largeRat,
            largeBat,
            meleeSkeleton,
            rangedSkeleton,
            rangedSkeleton,
            meleeSkeleton
        };
    }

    // Update is called once per frame
    void Update()
    {
        //start the enemy rush after the player moves to this position
        if(playerTrans.position.x > 0)
        {
            startEnemyRush = true;
        }

        //enemies come in when the rush starts. No more than 3 enemies in the arena at a time.
        if(startEnemyRush)
        {
            if(i < Enemies.Count)
            {
                enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
                noOfEnemies = enemiesArray.Length;

                if (noOfEnemies < 3)
                {
                    spawnPos.Set(Random.Range(ArenaLeftXCoordinate, ArenaRightXCoordinate), ArenaUpYCoordinate, 0);

                    Instantiate(Enemies[i], spawnPos, playerTrans.rotation, transform.parent);
                    i++;
                }
            }
            else
            {
                hasCompletedEnemyRush = true;
            }
        }
    }
}
