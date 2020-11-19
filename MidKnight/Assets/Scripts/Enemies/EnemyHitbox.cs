using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the hitbox of the enemies
/// </summary>
public class EnemyHitbox : MonoBehaviour
{
    /// <summary>
    /// returns a reference to the player
    /// </summary>
    Character player;
    /// <summary>
    /// returns a reference to the eneny
    /// </summary>
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// deal damage to the player when they come into contact with the enemy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.SetKnockBackDirection(player.transform.position - enemy.transform.position);
            player.TakeDamage(enemy.damage);
            Debug.Log("Enemy did damage");
        }
    }
}
