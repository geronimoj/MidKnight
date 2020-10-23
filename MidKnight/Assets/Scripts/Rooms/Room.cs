using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Room", order = 1)]
public class Room : ScriptableObject
{
    public GameObject roomPrefab;
    public Entrance[] entranceLocations;
    public Vector2[] pathNodes;
}

public struct Entrance
{
    Vector3 location;
    Vector3 direction;
}
