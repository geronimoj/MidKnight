using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Room", menuName = "Room", order = 1)]
public class Room : MonoBehaviour
{
    [SerializeField]
    private EntitiesManager EM;
    public string roomID = "newRoom";
    public List<GameObject> roomObjects;
    public Vector3[] entrances;
    public Vector2[] pathNodes;

    private void Start()
    {
        if (roomID == "newRoom")
        {
            Debug.LogError("RoomID not set.");
        }
    }

    public void InstantiateRoom()
    {
        EM = FindObjectOfType<EntitiesManager>();
        GameObject o = Instantiate(gameObject);
        Room r = o.GetComponent<Room>();

        foreach (Entities obj in EM.EntitiesToNotRespawn)
        {
            if (obj.thisRoom == r.roomID)
            {
                r.roomObjects[obj.index].SetActive(false);
            }
        }
    }
}
