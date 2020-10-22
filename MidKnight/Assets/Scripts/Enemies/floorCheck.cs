using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCheck : MonoBehaviour
{
    public bool isThereFloor = true;

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
        isThereFloor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isThereFloor = false;
    }
}
