using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    Transform playerTrans;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerTrans.Translate(0, speed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerTrans.Translate(0, -speed, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Translate(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Translate(-speed, 0, 0);
        }
    }
}
