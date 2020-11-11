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
    /// The length of the hitstun
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    protected float hitstunDuration = 0.1f;
    /// <summary>
    /// A timer for hitstun
    /// </summary>
    protected float hitstunTimer = 0;
    /// <summary>
    /// The duration of the knockback
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    protected float knockBackDuration = 0.1f;
    /// <summary>
    /// A timer for the knockBack
    /// </summary>
    protected float knockBackTimer = 0;
    /// <summary>
    /// The direction of knockback
    /// </summary>
    public Vector3 knockBackDir = Vector3.zero;
    /// <summary>
    /// How quickly the knockback moves the cahracter
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    public float knockBackForce = 0;
    /// <summary>
    /// Returns true if the character is currently in hitstun
    /// </summary>
    public bool InHitStun
    {
        get
        {
            return hitstunTimer > 0;
        }
    }
    /// <summary>
    /// Returns true if the character is currently being knocked back
    /// </summary>
    public bool BeingKnockedBack
    {
        get
        {
            return knockBackTimer > 0;
        }
    }

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
    private void Awake()
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

    private void Update()
    {
        hitstunTimer -= Time.deltaTime;
        if (hitstunTimer < 0)
            knockBackTimer -= Time.deltaTime;
        ExtraUpdate();
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

        knockBackTimer = knockBackDuration;
        hitstunTimer = hitstunDuration;

        if (Health <= 0)
            OnDeath();
    }
    /// <summary>
    /// An overrideable function for any extra update function calls
    /// </summary>
    protected virtual void ExtraUpdate()
    {
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
    /// <summary>
    /// A set for the direction of knockback. Is normalised by default
    /// </summary>
    /// <param name="dir">The direction of knockback</param>
    public virtual void SetKnockBackDirection(Vector3 dir)
    {
        knockBackDir = dir.normalized;
    }
}
