using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public GameObject currentRoom;
    public Room nextRoom;
    public EnemyManager EM;
    private uint entranceIndex;

    private void OnTriggerEnter()
    {
        Instantiate(nextRoom.roomPrefab);
        Destroy(currentRoom);
    }
}
