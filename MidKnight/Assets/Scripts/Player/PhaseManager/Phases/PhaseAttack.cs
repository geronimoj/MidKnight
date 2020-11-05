using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Default", order = 0)]
public class PhaseAttack : ScriptableObject
{   
    /// <summary>
    /// The damage of the attacks
    /// </summary>
    public uint damage;
    /// <summary>
    /// Called when an attack hits an enemy
    /// </summary>
    public UnityEvent OnHit;
    /// <summary>
    /// A storage location for enemies that were hit by attacks so they don't take damage multiple times
    /// </summary>
    protected List<GameObject> targetsHit = new List<GameObject>();
    /// <summary>
    /// A temporary value for displaying the attack animations until the Attacks are fully completed
    /// </summary>
    private float attackTimer = 0;
    /// <summary>
    /// The default attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DefaultAttack(PlayerController c)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= 1)
        {
            c.Attacking = false;
            c.animator.SetBool("Attacking", false);
            attackTimer = 0;
        }
    }
    /// <summary>
    /// The upwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void UpAttack(PlayerController c)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= 1)
        {
            c.Attacking = false;
            c.animator.SetBool("Attacking", false);
            attackTimer = 0;
        }
    }
    /// <summary>
    /// The downwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DownAttack(PlayerController c)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= 1)
        {
            c.Attacking = false;
            c.animator.SetBool("Attacking", false);
            attackTimer = 0;
        }
    }
}
