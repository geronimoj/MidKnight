using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocks : basePrefab
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        destination.Set(prefabTrans.position.x, -100, prefabTrans.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination(destination);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
