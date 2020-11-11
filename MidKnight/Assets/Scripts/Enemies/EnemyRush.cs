using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRush : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnPos = new Vector3(0, 0, 0);

        Enemies = new List<GameObject>()
        {
            rat,
            largeBat,
            largeRat,
            meleeSkeleton,
            rat,
            largeBat,
            rat,
            meleeSkeleton,
            rangedSkeleton,
            largeBat,
            rangedSkeleton,
            meleeSkeleton,
            rangedSkeleton,
            rat,
            largeRat
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTrans.position.x > 0)
        {
            startEnemyRush = true;
        }

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
        }
    }
}
