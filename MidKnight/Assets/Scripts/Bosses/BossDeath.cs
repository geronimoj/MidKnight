using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : StateMachineBehaviour
{
    public string unlockOnDeath = "NULL";
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        PlayerController.Player.GetComponent<UnlockTracker>().SetKey(unlockOnDeath, true);
    }
}
