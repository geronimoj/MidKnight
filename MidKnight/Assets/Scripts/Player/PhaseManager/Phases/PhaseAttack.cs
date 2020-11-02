using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Default", order = 0)]
public class PhaseAttack : ScriptableObject
{
    public uint damage;

    public UnityEvent OnHit;

    public void DefaultAttack(Transform origin, int bonusDamage)
    {

    }

    public void UpAttack(Transform origin, int bonusDamage)
    {

    }

    public void DownAttack(Transform origin, int bonusDamage)
    {

    }
}
