using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        bool Break = false;
        //if the roomID isn't set there are potential spawning problems
        if (roomID == "")
        {
            Debug.LogError("RoomID not set.");
            Break = true;
        }
        if (entrances.Length == 0)
        {
            Debug.LogError($"No Entrances on {roomID}.");
            Break = true;
        }

        int i = 0;

        foreach (Vector3 enter in entrances)
        {
            if (enter.Equals(new Vector3(0, 0, 0)))
            {
                Debug.LogWarning($"Entrance {i} on {roomID} may be wrong.");
            }

            i++;
        }

        if (Break)
        {
            Debug.Break();
        }
    }

    //Set up to instantiate a room whilst removing objects that shouldn't be there
    public void InstantiateRoom()
    {
        EM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EntitiesManager>();
        if (EM == null)
        {
            Debug.LogError("EntitiesManager not found on GameManager GameObject");
            Debug.Break();
            return;
        }
        Room r = Instantiate(gameObject).GetComponent<Room>();

        foreach (Entities obj in EM.EntitiesToNotRespawn)
        {
            if (obj.thisRoom == r.roomID)
            {
                r.NonRespawningRoomObjects[obj.index].SetActive(false);
            }
        }
    }
}
