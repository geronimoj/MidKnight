using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if this object is colliding with the floor 
/// </summary> 
public class floorCheck : MonoBehaviour
{
    /// <summary>
    /// returns true if there is floor colliding with this gameobject
    /// </summary>
    public bool isThereFloor;

    int numOfFloor = 0;

    // Start is called before the first frame update
    void Start()
    {
        isThereFloor = true;
        numOfFloor = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            isThereFloor = true;
            numOfFloor++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            numOfFloor--;
            if (numOfFloor <= 0)
                isThereFloor = false;
        }
    }
}
