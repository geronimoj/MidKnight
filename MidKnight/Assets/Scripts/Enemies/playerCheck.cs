using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCheck : MonoBehaviour
{
    public bool isTherePlayer;

    // Start is called before the first frame update
    void Start()
    {
        isTherePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isTherePlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTherePlayer = false;
        }
    }
}

