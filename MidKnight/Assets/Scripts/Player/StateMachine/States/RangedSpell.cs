using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedSpell", menuName = "States/RangedSpell", order = 10)]
public class RangedSpell : CastSpell
{
    public GameObject projectile;
    public int damage = 1;
    public float speed = 0;
    public Vector3 spawnOffset;

    protected override void ExtraOnEnter(ref PlayerController c)
    {
        //Call the spell animation
    }

    protected override void DoSpell(ref PlayerController c)
    {
        if (projectile == null)
        {
            Debug.LogError("Projectile on RangedSpell unassigned");
            return;
        }
        Vector3 spawnPos = c.transform.position + c.transform.right * spawnOffset.x + c.transform.forward * spawnOffset.y + c.transform.up * spawnOffset.y;

        Projectile p = Instantiate(projectile, spawnPos, Quaternion.identity).GetComponent<Projectile>();
        p.transform.rotation = c.transform.rotation;
        p.Damage = damage + c.BonusDamage;
        p.Speed = speed;
    }
}
