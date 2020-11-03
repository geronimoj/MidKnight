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
    /// The default attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DefaultAttack(ref PlayerController c)
    {

    }
    /// <summary>
    /// The upwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void UpAttack(ref PlayerController c)
    {

    }
    /// <summary>
    /// The downwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DownAttack(ref PlayerController c)
    {

    }
}
