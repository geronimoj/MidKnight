﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public Room nextRoom;
    private uint entranceIndex;

    public void LoadRoom()
    {

    }

    private void OnTriggerEnter()
    {
        LoadRoom();
    }
}
