using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public Room nextRoom;
    public EnemyManager EM;
    private uint entranceIndex;

    public void LoadRoom()
    {

    }

    private void OnTriggerEnter()
    {
        LoadRoom();
    }
}
