using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeTillDestroy : MonoBehaviour
{
    /// <summary>
    /// destroy the object after this timer
    /// </summary>
    public float startTimeTillDestroy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimeTillDestroy > 0)
        {
            startTimeTillDestroy -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
