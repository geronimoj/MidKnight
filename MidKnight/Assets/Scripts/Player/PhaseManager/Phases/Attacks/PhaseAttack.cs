using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PhaseAttack", menuName = "PhaseAttack/Default", order = 0)]
public class PhaseAttack : ScriptableObject
{   
    /// <summary>
    /// The attacks
    /// </summary>
    public Attack[] attacks;
    /// <summary>
    /// Called when an attack hits an enemy
    /// </summary>
    public UnityEvent OnHit;
    /// <summary>
    /// The timer used for the attacks
    /// </summary>
    private float attackTimer = 0;
    /// <summary>
    /// The default attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DefaultAttack(ref PlayerController c)
    {   //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(0, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, ref c, c.transform.right);
        //Check if the attack has finished
        if (attacks[0].AttackFinished)
            AttackFinished(ref c);
    }
    /// <summary>
    /// The upwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void UpAttack(ref PlayerController c)
    {
        //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(1, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, ref c, Vector3.up);
        //Did the attack finish?
        if (attacks[1].AttackFinished)
            AttackFinished(ref c);
    }
    /// <summary>
    /// The downwards attack
    /// </summary>
    /// <param name="c">A reference to the player controller. Can retrive bonus damage and positional data from it</param>
    public virtual void DownAttack(ref PlayerController c)
    {   //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(2, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, ref c, Vector3.down);
        //Do a pogo on the enemies hit
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.CompareTag("Enemy"))
            {
                c.movement.VertSpeed = c.PogoForce;
                c.OnLand();
            }
        //Has the attack finished?
        if (attacks[2].AttackFinished)
            AttackFinished(ref c);
    }
    /// <summary>
    /// An encapsulated function for performing the raycast and dealing damage to hit enemies.
    /// Returns the hit gameobjects incase other affects need to be applied to them.
    /// Targets already hit from previous raycasts are not included in the return
    /// </summary>
    /// <param name="attackIndex">Which attack should be used in the attacks array</param>
    /// <param name="c">A reference to the player controller</param>
    /// <returns>Returns the hit enemies if they haven't already been hit</returns>
    protected RaycastHit[] GetAttackHit(int attackIndex, ref PlayerController c)
    {
        //Do we have a vaid index for this attack?
        if (attacks.Length < attackIndex + 1 || attacks[attackIndex] == null)
        {
            AttackFailed(ref c, "Attack at index " + attackIndex + " not assigned");
            return null;
        }
        //Perform the attack
        RaycastHit[] hits = attacks[attackIndex].DoAttack(ref attackTimer, c.transform);
        //Does damage
        DealDamage(ref hits, ref c, c.BonusDamage);

        return hits;
    }

    protected void ApplyKnockback(ref RaycastHit[] hits, ref PlayerController c, Vector3 pushDir)
    {
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.CompareTag("Enemy"))
            {
                Enemy e = hits[i].transform.GetComponent<Enemy>();
                //Get the enemy component from the enemy and knock them backwards
                e.SetKnockBackDirection(pushDir);
                e.knockBackForce = c.Knockback;
            }
    }
    /// <summary>
    /// Deals damage to the hit targets
    /// </summary>
    /// <param name="hits">The hit targets</param>
    /// <param name="c">A reference to the player controller for moonLight Gain</param>
    /// <param name="bonusDamage">Any bonus damage to add</param>
    protected void DealDamage(ref RaycastHit[] hits, ref PlayerController c, int bonusDamage)
    {   //Make sure hits is valid
        if (hits == null)
            return;
        //Deal damage to targets if they are enemies
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.CompareTag("Enemy"))
            {   //Get the enemy component from the enemy
                Enemy e = hits[i].transform.GetComponent<Enemy>();
                //Make sure the enemy has the component
                if (e == null)
                {   //Log a warning as it is possible that some enemies may not have this script for some reason in future
                    Debug.LogWarning("Enemy " + hits[i].transform.name + " has no Enemy script and cannot be damaged.");
                    continue;
                }
                //Deal damage to the enemy
                e.TakeDamage(c.Damage + bonusDamage);
                c.MoonLight += c.MoonLightGain;
                OnHit.Invoke();
            }
    }
    /// <summary>
    /// An encapsulated function for performing all the necessary tasks when finishing an attack
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    public void AttackFinished(ref PlayerController c)
    {
        c.Attacking = false;
        c.animator.SetBool("Attacking", false);
        attackTimer = 0;
    }
    /// <summary>
    /// An encapsulated function for logging and dealing with the attack if it reaches any given fail state.
    /// Causes the attack to exit.
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    /// <param name="error">The error to spit into the console</param>
    protected void AttackFailed(ref PlayerController c, string error)
    {
        Debug.LogError(error);
        AttackFinished(ref c);
    }
}
