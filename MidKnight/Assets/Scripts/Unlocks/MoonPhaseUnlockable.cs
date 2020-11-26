using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPhaseUnlockable : Unlockable
{
    private GameObject player;

    protected void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        foreach (string powerUP in powerUP)
        {
            if (powerUP.ToLower() == "new moon")
            {
                player.GetComponent<PhaseManager>().swapToIndex = 0;
            }
            else if (powerUP.ToLower() == "half moon")
            {
                player.GetComponent<PhaseManager>().swapToIndex = 0;
            }
            else if (powerUP.ToLower() == "crescent")
            {
                player.GetComponent<PhaseManager>().swapToIndex = 0;
            }
            else if (powerUP.ToLower() == "full moon")
            {
                player.GetComponent<PhaseManager>().swapToIndex = 0;
            }
        }
    }
}
