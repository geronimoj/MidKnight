using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoonPhase", menuName = "MoonPhases/Empty", order = 0)]
public class MoonPhase : ScriptableObject
{
    public string phaseID = "NULL";

    public float phaseCooldown = 0;
    private float cooldownTimer = 0;

    public PhaseAttack attack;

    public PhaseAttack Attacks
    {
        get
        {
            return attack;
        }
    }

    public bool OnCooldown
    {
        get
        {
            return cooldownTimer <= 0;
        }
    }

    public UnityEvent OnEnter;

    public UnityEvent OnExit;

    public virtual void PhaseEnter(ref PlayerController c)
    {
        Debug.LogWarning("Empty Phase");
    }

    public void DoPhase(ref PlayerController c)
    {
        cooldownTimer -= Time.deltaTime;

        PhaseUpdate(ref c);
    }

    public virtual void PhaseUpdate(ref PlayerController c) { }

    public virtual void PhaseExit(ref PlayerController c) { }
}
