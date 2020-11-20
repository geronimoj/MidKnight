using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    
    public float ScrollX = 0f;
    public float ScrollY = 0f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        rend.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
