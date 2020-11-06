using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMoon", menuName = "Phases/NewMoon", order = 0)]
public class NewMoon : MoonPhase
{
    /// <summary>
    /// The moonlight to give the player
    /// </summary>
    [Range(0, 1000)]
    public float bonusMoonLight = 0;
    /// <summary>
    /// Gives the player moonlight if this is the active phase
    /// </summary>
    public void GiveMoonLight()
    {
        if (pc == null || !Active)
            return;
        pc.MoonLight += bonusMoonLight;
    }
}
