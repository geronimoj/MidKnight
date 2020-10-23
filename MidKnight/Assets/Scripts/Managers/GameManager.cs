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
    /// Returns the direction of the path to the right
    /// </summary>
    /// <param name="position">The position of the player</param>
    /// <returns>A unit vector pointing along the path to the right along the horizontal plane</returns>
    public Vector3 GetPathDirection(Vector3 position)
    {
        return Vector3.right;
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
    /// <summary>
    /// Takes the position and movement vector and returns a new movement vector that will end the character along the path
    /// </summary>
    /// <param name="position">The position of the character</param>
    /// <param name="moveVector">The movement vector of the character</param>
    /// <returns>The new movement vector</returns>
    public Vector3 MoveAlongPath(Vector3 position, Vector3 moveVector)
    {
        return moveVector;
    }
}
