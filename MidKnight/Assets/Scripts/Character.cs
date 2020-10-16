using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    /// <summary>
    /// The current health of the character
    /// </summary>
    protected int health;
    /// <summary>
    /// The max health of the character, health cannot exceed this value
    /// It is stored as a float to make it easier to increment
    /// </summary>
    [SerializeField]
    private float maxHealth;
    /// <summary>
    /// A get for maxHealth
    /// </summary>
    public int MaxHealth
    {
        get
        {
            return (int)maxHealth;
        }
    }
    /// <summary>
    /// A Set/Get for health. Automatically clamps for maxHealth
    /// </summary>
    private int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health > MaxHealth)
                health = MaxHealth;
        }
    }
    /// <summary>
    /// A reference to the Character Controller
    /// </summary>
    protected CharacterController cc;
    /// <summary>
    /// Gets the character controller
    /// </summary>
    public void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    /// <summary>
    /// An overridable take damage function
    /// </summary>
    /// <param name="damage">The damage to take</param>
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
    }
    /// <summary>
    /// Moves the character or object
    /// </summary>
    /// <param name="moveVec"></param>
    public void Move(Vector3 moveVec)
    {

    }
}
