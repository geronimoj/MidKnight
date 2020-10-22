using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Room room;
    /// <summary>
    /// Snaps the position given onto the path
    /// INCOMPLETE
    /// </summary>
    /// <param name="position">The current position of the object</param>
    /// <returns>The new position of the object</returns>
    public Vector3 SnapToPath(Vector3 position)
    {
        if (room == null || room.pathNodes.Length < 2)
        {
            position.z = 0;
            return position;
        }
        //Temporary
        return position;
    }
    /// <summary>
    /// Returns the direction the path is moving in
    /// INCOMPLETE
    /// </summary>
    /// <param name="lookingRight">Do you want the direction of the path to the left or right</param>
    /// <returns>Returns a unit vector along the path. Does not account for veritcal movement</returns>
    public Vector3 GetPathDirection(Vector3 position, bool lookingRight)
    {
        if (room == null || room.pathNodes.Length < 2)
        {
            if (lookingRight)
                return Vector3.right;
            else
                return Vector3.left;
        }
        //FILL OUT
        return Vector3.zero;
    }
    /// <summary>
    /// Gets the path for readonly purposes
    /// </summary>
    /// <returns>An array of the nodes of the path</returns>
    public Vector2[] GetPath()
    {
        if (room != null && room.pathNodes != null)
            return room.pathNodes;
        return null;
    }
}
