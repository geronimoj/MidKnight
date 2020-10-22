using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCheck : MonoBehaviour
{
    /// <summary>
    /// Check if this object is colliding with the floor 
    /// </summary> 
    
    public bool isThereFloor;

    // Start is called before the first frame update
    void Start()
    {
        isThereFloor = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            isThereFloor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            isThereFloor = false;
        }
    }
}
