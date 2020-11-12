using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeTillColliderEnabled : MonoBehaviour
{
    /// <summary>
    /// turn on the collider after this timer
    /// </summary>
    Collider prefabCol;
    public float timeTillEnabled;

    // Start is called before the first frame update
    void Start()
    {
        prefabCol = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeTillEnabled > 0)
        {
            timeTillEnabled -= Time.deltaTime;
        }
        else
        {
            prefabCol.enabled = true;
        }
    }
}
