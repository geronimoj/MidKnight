using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExitSpawner : MonoBehaviour
{
    /// <summary>
    /// The ID of the unlock
    /// </summary>
    public string unlockID = "NULL";
    /// <summary>
    /// A reference to the roomExit to deactivate
    /// </summary>
    public GameObject roomExit;

    void Update()
    {
        if (roomExit == null)
            return;
        //Checks if the ID has been unlocked
        if (PlayerController.Player.ut.GetKeyValue(unlockID))
            //Unlock the exit
            roomExit.SetActive(true);
        else
            //Hide the exit
            roomExit.SetActive(false);
    }
}
