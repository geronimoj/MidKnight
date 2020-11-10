using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   /// <summary>
/// The hitbox that will be used for collision detection and damage
/// </summary>
    public Attack hitBox;
    /// <summary>
    /// The speed of the projectile. Set through Speed
    /// </summary>
    private float speed = 1;
    /// <summary>
    /// A set for speed
    /// </summary>
    public float Speed
    {
        set
        {
            speed = value;
        }
    }
    /// <summary>
    /// The damage of the projectile. Set through damage
    /// </summary>
    private int damage = 0;
    /// <summary>
    /// A set for damage
    /// </summary>
    public int Damage
    {
        set
        {
            damage = value;
        }
    }
    /// <summary>
    /// The timer for the attack
    /// </summary>
    private float timer = 0;
    /// <summary>
    /// A storage location for the movementVector so we don't have to keep creating it
    /// </summary>
    private Vector3 moveVec;

    private bool destroySelf = false;
    /// <summary>
    /// A reference to the gameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Gets references to everything and doesn't smash stuff
    /// </summary>
    private void Start()
    {   //Make sure we have a hitbox we can change
        if (hitBox == null)
        {
            Debug.LogError("Hitbox not assigned to projectile");
            Debug.Break();
        }

        if (hitBox.hitboxes.Length == 0)
            hitBox.hitboxes = new Hitbox[1];

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gm == null)
            Debug.LogError("GameManager not found in scene. Expected to be tagged GameManager");
        //Don't destroy yourself
        destroySelf = false;
    }
    /// <summary>
    /// Moves and checks for collision on the projectile
    /// </summary>
    private void Update()
    {
        //If we should destroy ourself. DO IT
        //We do this at the beginning of the frame so it has a chance to update its visual location before destroying itself
        if (destroySelf)
            Destroy(gameObject);
        //Calculate the vector of movement
        moveVec = transform.right * speed * Time.deltaTime;

        moveVec = gm.MoveAlongPath(transform.position, moveVec);
        //Make sure we raycast the hitbox along the path we move, not just the end point
        hitBox.hitboxes[0].endPoint = moveVec;
        //Update the time so it raycasts for the full duration
        hitBox.hitboxes[0].endTime = timer + Time.deltaTime;
        //Set the timer to 0 so it clears the hit targets of the hitBox attack.
        //Otherwise if multiple projectiles exist, they will not collide with terrain a previous projectile did
        //Scriptable objects :P
        //This does mean that it spam calls OnStart for the attack
        timer = 0;

        RaycastHit[] r = hitBox.DoAttack(ref timer, transform);
        //Should the projectile destroy itself yet?
        if (hitBox.AttackFinished)
            destroySelf = true;

        foreach (RaycastHit hit in r)
        {   //Don't collide with the player
            if (!hit.transform.CompareTag("Player"))
                destroySelf = true;
            else
                continue;
            //If its an enemy, deal damage
            if (hit.transform.CompareTag("Enemy"))
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
        }
        //Move the projectile since we are destroying it when it collides with anything, we don't need a character controller
        //The hitBox acts for both damage and as the hitbox
        transform.position += moveVec;
        //Rotate the projectile to look along its path of movement
        moveVec.y = 0;
        if (moveVec != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(-moveVec.z, 0, moveVec.x), Vector3.up);
    }
}
