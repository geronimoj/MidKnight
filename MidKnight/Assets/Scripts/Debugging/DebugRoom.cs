using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRoom : MonoBehaviour
{   
    /// <summary>
    /// The room you want to debug
    /// </summary>
    [Tooltip("The room you want to debug")]
    public Room room;
    /// <summary>
    /// The radius of the pathNodes
    /// </summary>
    public float pointRadius;
#if !UNITY_EDITOR
    private void Awake()
    {
        Destroy(this);
    }
#endif
    /// <summary>
    /// Draws the Path and PathNodes
    /// </summary>
    private void OnDrawGizmos()
    {   //If we don't have a room, don't bother
        if (room == null)
            return;
        //Loop through the pathNodes and display them
        for (int i = 0; i < room.pathNodes.Length; i++)
        {   //Draw the pathNodes with a colour
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PNodeToV3(room.pathNodes[i]), pointRadius);
            //If we have a segment, draw the segment
            if (i < room.pathNodes.Length - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(PNodeToV3(room.pathNodes[i]), PNodeToV3(room.pathNodes[i + 1]));
            }
        }
    }
    /// <summary>
    /// Converts the cords of a PathNode to Vector3 cords
    /// </summary>
    /// <param name="v">The PathNode to convert</param>
    /// <returns>The PathNodes cords as a V3</returns>
    private Vector3 PNodeToV3 (Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }
}
