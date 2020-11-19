using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// all prefabs will derive from this
/// </summary>
public class basePrefab : MonoBehaviour
{
    /// <summary>
    /// a reference to the player's transform
    /// </summary>
    [HideInInspector] public Transform playerTrans;
    /// <summary>
    /// a reference to the prefab's transform
    /// </summary>
    [HideInInspector] public Transform prefabTrans;
    /// <summary>
    /// the prefab's destination
    /// </summary>
    [HideInInspector] public Vector3 destination = new Vector3(0,0,0);
    /// <summary>
    /// a reference to the player
    /// </summary>
    Character player;
    /// <summary>
    /// a reference to the prefab's timetilldestroy script
    /// </summary>
    timeTillDestroy timeTillDestroy;
    /// <summary>
    /// the prefab's speed
    /// </summary>
    public int speed = 1;
    /// <summary>
    /// the prefab's damage
    /// </summary>
    public int damage = 1;
    /// <summary>
    /// returns true if the prefab is breakable
    /// </summary>
    public bool isBreakable = false;
    /// <summary>
    /// a reference to the prefab's collider
    /// </summary>
    Collider prefabCol;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //initialise stuff
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        prefabTrans = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        timeTillDestroy = GetComponent<timeTillDestroy>();
        prefabCol = GetComponent<Collider>();
    }

    /// <summary>
    /// check if the player is on the right of the prefab
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// deal damage to the player when they come into contact
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Debug.Log("Skill did damage");
        }
    }

    /// <summary>
    /// Make the prefab move to this destination
    /// </summary>
    /// <param name="destination"></param>
    public void MoveToDestination(Vector3 destination)
    {
        prefabTrans.position = Vector3.MoveTowards(prefabTrans.position, destination, speed * Time.deltaTime);
    }

    /// <summary>
    /// Make the prefab move to this destination at a different speed
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public void MoveToDestination(Vector3 destination, int speed)
    {
        prefabTrans.position = Vector3.MoveTowards(prefabTrans.position, destination, speed * Time.deltaTime);
    }

    /// <summary>
    /// make the prefab face left
    /// </summary>
    public void FaceLeft()
    {
        prefabTrans.eulerAngles = new Vector3(180, 0, 0);
    }

    /// <summary>
    /// make the prefab face right
    /// </summary>
    public void FaceRight()
    {
        prefabTrans.eulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// check if the prefab has been hit and destroy it if it can be destroyed
    /// </summary>
    public virtual void HasBeenHit()
    {
        if (isBreakable)
        {
            destination.Set(prefabTrans.position.x, prefabTrans.position.y, prefabTrans.position.z);
            prefabCol.enabled = false;
            timeTillDestroy.enabled = true;
            timeTillDestroy.startTimeTillDestroy = 1f;
        }
    }
}
