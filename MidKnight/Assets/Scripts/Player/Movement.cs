using UnityEngine;

public struct Movement
{
    private Vector3 movement;
    private Vector3 direction;
    /// <summary>
    /// Sets or Gets the vertical speed
    /// </summary>
    public float VertSpeed
    {
        get
        {
            return movement.y;
        }
        set
        {
            movement.y = value;
            Direction = movement.normalized;
        }
    }
    /// <summary>
    /// Sets or Gets the horizontal speed
    /// </summary>
    public float HozSpeed
    {
        get
        {
            return new Vector3(movement.x, 0, movement.z).magnitude;
        }
        set
        {
            movement.x = value * direction.x;
            movement.z = value * direction.z;
        }
    }
    /// <summary>
    /// Sets or Gets the total speed (horizontal and vertical)
    /// </summary>
    public float Speed
    {
        get
        {
            return movement.magnitude;
        }
        set
        {
            movement = direction * value;
        }
    }
    /// <summary>
    /// Sets or Gets the direction movement is pointing in
    /// </summary>
    public Vector3 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value.normalized;
            movement = direction * movement.magnitude;
        }
    }
    /// <summary>
    /// Gets the total movement vector (speed * direction)
    /// </summary>
    public Vector3 MoveVec
    {
        get
        {
            return movement;
        }
    }
}
