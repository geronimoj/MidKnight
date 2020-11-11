using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullMoonAttack", menuName = "PhaseAttack/FullMoon", order= 1)]
public class FullMoonAttack : PhaseAttack
{
    public override void DefaultAttack(ref PlayerController c)
    {   //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(0, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, c.transform.right);
        //Check if the hit objects are skills
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.CompareTag("Skill"))
            {
                basePrefab bP = hits[i].transform.GetComponent<basePrefab>();
                if (bP == null)
                    return;
                bP.HasBeenHit();
            }
        //Check if the attack has finished
        if (attacks[0].AttackFinished)
            AttackFinished(ref c);
    }

    public override void DownAttack(ref PlayerController c)
    {   //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(2, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, Vector3.down);
        //Do a pogo on the enemies hit
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                c.movement.VertSpeed = pogoForce;
                c.OnLand();
                continue;
            }

            if (hits[i].transform.CompareTag("Skill"))
            {
                basePrefab bP = hits[i].transform.GetComponent<basePrefab>();
                if (bP == null)
                    return;
                bP.HasBeenHit();
            }
        }
        //Has the attack finished?
        if (attacks[2].AttackFinished)
            AttackFinished(ref c);
    }

    public override void UpAttack(ref PlayerController c)
    {   //Calls the raycast & does damage.
        RaycastHit[] hits = GetAttackHit(0, ref c);
        //Apply other affects
        ApplyKnockback(ref hits, Vector3.up);
        //Check if the hit objects are skills
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.CompareTag("Skill"))
            {
                basePrefab bP = hits[i].transform.GetComponent<basePrefab>();
                if (bP == null)
                    return;
                bP.HasBeenHit();
            }
        //Check if the attack has finished
        if (attacks[0].AttackFinished)
            AttackFinished(ref c);
    }
}
