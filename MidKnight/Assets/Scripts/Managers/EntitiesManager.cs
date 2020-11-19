using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class EntitiesManager : MonoBehaviour
{
    public List<Entities> EntitiesToNeverRespawn = new List<Entities>();
    public List<Entities> EntitiesToNotRespawnUntillRest = new List<Entities>();
}

[System.Serializable]
public struct Entities
{
    public Entities(int Index = 0, string RoomID = "")
    {
        index = Index;
        thisRoom = RoomID;
    }

    public int index;
    public string thisRoom;
}
