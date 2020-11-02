using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class PhaseManager : MonoBehaviour
{
    public float swapCooldown = 0;
    private float cooldownTimer = 0;
    [SerializeField]
    private MoonPhase current;
    public UnityEvent OnSwap;

    public MoonPhase CurrentPhase
    {
        get
        {
            return current;
        }
    }

    public void PhaseUpdate(PlayerController c)
    {
        cooldownTimer -= Time.deltaTime;
        if (current != null)
            current.DoPhase(ref c);
#if UNITY_EDITOR
        else
            Debug.LogWarning("No State");
#endif
    }

    public void PhaseStart(PlayerController c)
    {
        current.PhaseEnter(ref c);
        current.OnEnter.Invoke();
    }

    public void SwapPhase(MoonPhase target, PlayerController c)
    {
        if (cooldownTimer > 0)
            return;
        cooldownTimer = swapCooldown;
        current.PhaseExit(ref c);
        current.OnExit.Invoke();

        current = target;
        OnSwap.Invoke();

        current.PhaseEnter(ref c);
        current.OnEnter.Invoke();
    }

    public bool CorrectPhase(string phaseID)
    {
        return current.phaseID.Equals(phaseID);
    }
#if UNITY_EDITOR
    public uint attackToDraw = 0;

    private void OnDrawGizmosSelected()
    {
        
    }
#endif
}
