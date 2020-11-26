using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoonPhase : MonoBehaviour
{
    public Material newMoon;
    public Material halfMoon;
    public Material crescentMoon;
    public Material fullMoon;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player.GetComponent<PhaseManager>().CurrentPhase.phaseID.ToLower() == "new moon")
        {
            GetComponent<MeshRenderer>().sharedMaterial = newMoon;
        }
        if (player.GetComponent<PhaseManager>().CurrentPhase.phaseID.ToLower() == "half moon")
        {
            GetComponent<MeshRenderer>().sharedMaterial = halfMoon;
        }
        if (player.GetComponent<PhaseManager>().CurrentPhase.phaseID.ToLower() == "crescent")
        {
            GetComponent<MeshRenderer>().sharedMaterial = crescentMoon;
        }
        if (player.GetComponent<PhaseManager>().CurrentPhase.phaseID.ToLower() == "full moon")
        {
            GetComponent<MeshRenderer>().sharedMaterial = fullMoon;
        }
    }
}
