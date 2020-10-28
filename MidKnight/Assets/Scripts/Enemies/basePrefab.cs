using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePrefab : MonoBehaviour
{
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Transform enemyTrans;
    [HideInInspector] public Transform prefabTrans;
    [HideInInspector] public Vector3 destination = new Vector3(0,0,0);
    public int speed = 1;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTrans = GetComponent<characterOwner>().Owner.GetComponent<Transform>();
        prefabTrans = GetComponent<Transform>();

        if(PlayerIsOnRight())
        {
            prefabTrans.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            prefabTrans.eulerAngles = new Vector3(180, 0, 0);
        }
    }

    public bool PlayerIsOnRight()
    {
        if(playerTrans.position.x > enemyTrans.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Make the enemy move to this destination
    /// </summary>
    /// <param name="destination"></param>
    public void MoveToDestination(Vector3 destination)
    {
        prefabTrans.position = Vector3.MoveTowards(prefabTrans.position, destination, speed * Time.deltaTime);
    }
}
