using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Room", order = 1)]
public class Room : ScriptableObject
{
    public GameObject roomPrefab;
    public Vector3[] entrances;
    public Vector2[] pathNodes;
}
