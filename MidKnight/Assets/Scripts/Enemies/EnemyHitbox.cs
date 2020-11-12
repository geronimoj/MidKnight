using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    /// <summary>
    /// the hitbox of the enemies
    /// </summary>
    PlayerController player;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
