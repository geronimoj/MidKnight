using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// turn on the collider after this timer
/// </summary>
public class timeTillColliderEnabled : MonoBehaviour
{
    /// <summary>
    /// reference to a collider of the prefab
    /// </summary>
    Collider prefabCol;
    /// <summary>
    /// enable the collider after this time
    /// </summary>
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
            prefabCol.transform.localScale = new Vector3(1, 1, 30);
        }
    }
}
