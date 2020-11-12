using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRush : MonoBehaviour
{
    /// <summary>
    /// the script for the enemy rush
    /// </summary>
    /// 
    public GameObject rat;
    public GameObject largeRat;
    public GameObject bat;
    public GameObject largeBat;
    public GameObject meleeSkeleton;
    public GameObject rangedSkeleton;
    public float ArenaLeftXCoordinate;
    public float ArenaRightXCoordinate;
    public float ArenaUpYCoordinate;
    Vector3 spawnPos;
    bool startEnemyRush = false;
    Transform playerTrans;
    List<GameObject> Enemies;
    int i = 0;
    GameObject[] enemiesArray;
    int noOfEnemies;
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

                    Instantiate(Enemies[i], spawnPos, playerTrans.rotation);
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
