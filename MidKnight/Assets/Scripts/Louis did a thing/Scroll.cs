using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    
    public float xMultiplier = 0.1f;
    public float yMultiplier = 0f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        PlayerController pc = PlayerController.Player;

        if (pc == null)
            return;

        Vector2 pos = pc.transform.position;
        pos.x *= -xMultiplier;
        pos.y *= -yMultiplier;

        rend.material.SetTextureOffset("_BaseMap",pos);
    }
}
