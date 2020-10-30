using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    /// <summary>
    /// Check if this object is colliding with a wall 
    /// </summary>

    public bool isThereAWall;

    // Start is called before the first frame update
    void Start()
    {
        isThereAWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            isThereAWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isThereAWall = false;
        }
    }
}
