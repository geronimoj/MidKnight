using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePrefab : MonoBehaviour
{
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Transform prefabTrans;
    [HideInInspector] public Vector3 destination = new Vector3(0,0,0);
    PlayerController player;
    public int speed = 1;
    public int damage = 1;
    public bool isBreakable = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        prefabTrans = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public bool PlayerIsOnRight()
    {
        if(playerTrans.position.x > prefabTrans.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Debug.Log("Skill did damage");
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

    public void MoveToDestination(Vector3 destination, int speed)
    {
        prefabTrans.position = Vector3.MoveTowards(prefabTrans.position, destination, speed * Time.deltaTime);
    }

    public void FaceLeft()
    {
        prefabTrans.eulerAngles = new Vector3(180, 0, 0);
    }

    public void FaceRight()
    {
        prefabTrans.eulerAngles = new Vector3(0, 0, 0);
    }

    public virtual void HasBeenHit()
    {
        if (isBreakable)
        {

        }
    }
}
