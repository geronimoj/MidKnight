using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private EntitiesManager EM;
    public string roomID = "";
    public List<GameObject> NonRespawningRoomObjects;
    public Vector3[] entrances;
    public Vector2[] pathNodes;
    public AudioSource music;

    private void Start()
    {
        if (music != null)
            music.Play();
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
    public void InstantiateRoom(ref GameManager gm)
    {
        EM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EntitiesManager>();
        if (EM == null)
        {
            Debug.LogError("EntitiesManager not found on GameManager GameObject");
            Debug.Break();
            return;
        }
        Room r = Instantiate(gameObject).GetComponent<Room>();
        gm.room = r;
        foreach (Entities obj in EM.EntitiesToNotRespawnUntillRest)
        {
            if (obj.thisRoom == r.roomID)
            {
                if (obj.index == -1)
                {
                    Debug.LogError("Entity does not exist in NonRespawningRoomObjects");
                    continue; 
                }
                r.NonRespawningRoomObjects[obj.index].SetActive(false);
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// The radius of the debugged points
    /// </summary>
    [Range(0.1f, 10)]
    public float pointRadius;
    /// <summary>
    /// Draws the Path and PathNodes
    /// </summary>
    private void OnDrawGizmos()
    {   
        //Loop through the pathNodes and display them
        for (int i = 0; i < pathNodes.Length; i++)
        {   //Draw the pathNodes with a colour
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PNodeToV3(pathNodes[i]), pointRadius);
            //If we have a segment, draw the segment
            if (i < pathNodes.Length - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(PNodeToV3(pathNodes[i]), PNodeToV3(pathNodes[i + 1]));
            }
        }
        //Draw the entrance locations
        Gizmos.color = Color.green;
        for (int i = 0; i < entrances.Length; i++)
        {
            Gizmos.DrawWireSphere(entrances[i], pointRadius);
        }
    }
    /// <summary>
    /// Converts the cords of a PathNode to Vector3 cords
    /// </summary>
    /// <param name="v">The PathNode to convert</param>
    /// <returns>The PathNodes cords as a V3</returns>
    private Vector3 PNodeToV3(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }
#endif
}
