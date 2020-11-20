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

    // Start is called before the first frame update
    void Start()
    {
        isThereFloor = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            isThereFloor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            isThereFloor = false;
        }
    }
}
