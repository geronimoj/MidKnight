using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedSpell", menuName = "States/RangedSpell", order = 10)]
public class RangedSpell : CastSpell
{
    public GameObject projectile;
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

        Instantiate(projectile, spawnPos, Quaternion.identity);
    }
}
