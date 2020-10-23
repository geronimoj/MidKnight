using UnityEngine;

[System.Serializable]
public struct Movement
{
    /// <summary>
    /// The storage location for the players movement
    /// </summary>
    [SerializeField]
    private Vector3 movement;
    /// <summary>
    /// The direction of the players movement
    /// </summary>
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
            movement = new Vector3(direction.x * HozSpeed, VertSpeed, direction.z * HozSpeed);
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
