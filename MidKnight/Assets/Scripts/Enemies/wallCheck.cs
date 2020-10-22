using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    public bool isThereAWall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isThereAWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isThereAWall = false;
    }
}
