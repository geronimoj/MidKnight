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
    [Range(1, 200)]
    private float maxHealth = 1;
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
    /// A Get for health. Automatically clamps for maxHealth
    /// </summary>
    public int Health
    {
        get
        {
            return health;
        }
    }
    /// <summary>
    /// A private Set for health to make things cleaner
    /// </summary>
    protected int SetHealth
    {
        set
        {
            health = value;
            //Clamp the health
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
        health = MaxHealth;

        if (cc == null)
        {
            Debug.LogWarning("Could not find character controller. Will be unable to move with Move function");
            Debug.Break();
        }
        AwakeExtra();
    }
    /// <summary>
    /// Any additional awake calls because the awake function in here requires to do its own stuff
    /// </summary>
    protected virtual void AwakeExtra() { }
    /// <summary>
    /// An overridable take damage function
    /// </summary>
    /// <param name="damage">The damage to take</param>
    public virtual void TakeDamage(int damage)
    {
        SetHealth = Health - damage;

        if (Health <= 0)
            OnDeath();
    }
    /// <summary>
    /// Moves the character or object
    /// </summary>
    /// <param name="moveVec">The movement vector</param>
    public virtual void Move(Vector3 moveVec)
    {
        if (cc == null)
            return;
        cc.Move(moveVec);
    }

    public virtual void OnDeath()
    {
        Debug.Log("I am dead");
    }
}
