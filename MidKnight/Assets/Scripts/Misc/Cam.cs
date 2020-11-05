using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform player;

    //Allows us to adjust the X, Y and Z values in unity
    public Vector3 cameraOffset;

    //Can adjust cameraSpeed to suit,
    //lower numbers will make the camera more "floaty" higher numbers are a more "rigid" camera,
    //can also be changed in unity
    public float cameraSpeed = 0.1f;

    void Start()
    {
        transform.position = player.position + cameraOffset;
    }

    void Update()
    {
        Vector3 finalPosition = player.position + cameraOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        transform.position = currentPosition;
    }
}
