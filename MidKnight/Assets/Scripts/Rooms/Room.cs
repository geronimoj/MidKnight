using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Room", menuName = "Room", order = 1)]
public class Room : MonoBehaviour
{
    [SerializeField]
    private EntitiesManager EM;
    public string roomID = "";
    public List<GameObject> NonRespawningRoomObjects;
    public Vector3[] entrances;
    public Vector2[] pathNodes;

    private void Start()
    {
        //if the roomID isn't set there are potential spawning problems
        if (roomID == "")
        {
            Debug.LogError("RoomID not set.");
            Debug.Break();
        }
    }

    //Set up to instantiate a room whilst removing objects that shouldn't be there
    public void InstantiateRoom()
    {
        EM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EntitiesManager>();
        GameObject o = Instantiate(gameObject);
        Room r = o.GetComponent<Room>();

        foreach (Entities obj in EM.EntitiesToNotRespawn)
        {
            if (obj.thisRoom == r.roomID)
            {
                r.NonRespawningRoomObjects[obj.index].SetActive(false);
            }
        }
    }
}
