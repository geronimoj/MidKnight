using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class EntitiesManager : MonoBehaviour
{
    public List<Entities> EntitiesToNotRespawn = new List<Entities>();
}

[System.Serializable]
public struct Entities
{
    public int index;
    public string thisRoom;
}
