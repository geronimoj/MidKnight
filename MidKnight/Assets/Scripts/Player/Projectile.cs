using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Attack hitBox;
    private int damage = 0;

    public int Damage
    {
        set
        {
            damage = value;
        }
    }

    private float timer = 0;

    private void Update()
    {
        RaycastHit[] r = hitBox.DoAttack(ref timer, transform);
        bool destorySelf = false;

        foreach (RaycastHit hit in r)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
                destorySelf = true;
            }
        }

        if (destorySelf)
            Destroy(gameObject);
    }
}
