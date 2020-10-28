using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamController : MonoBehaviour
{
    private GameObject target;
    public Vector3 offset;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
