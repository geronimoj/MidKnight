using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Snaps the position given onto the path
    /// INCOMPLETE
    /// </summary>
    /// <param name="position">The current position of the object</param>
    /// <returns>The new position of the object</returns>
    public Vector3 SnapToPath(Vector3 position)
    {
        return position;
    }
    /// <summary>
    /// Returns the direction the path is moving in
    /// INCOMPLETE
    /// </summary>
    /// <param name="lookingRight">Do you want the direction of the path to the left or right</param>
    /// <returns>Returns a unit vector along the path. Does not account for veritcal movement</returns>
    public Vector3 GetPathDirection(bool lookingRight)
    {
        if (lookingRight)
            return Vector3.right;
        else
            return Vector3.left;
    }
    /// <summary>
    /// Gets the path for readonly purposes
    /// </summary>
    /// <returns>An array of the nodes of the path</returns>
    public Vector2[] GetPath()
    {
        return null;
    }
}
