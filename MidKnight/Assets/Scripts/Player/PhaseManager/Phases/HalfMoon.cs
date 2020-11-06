using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HalfMoon", menuName = "Phases/HalfMoon", order= 3)]
public class HalfMoon : MoonPhase
{
    /// <summary>
    /// How long the player should have iFrames for when dashing
    /// </summary>
    [Range(0,100f)]
    public float dashIFrameDuration = 0;
    /// <summary>
    /// Gives the player IFrames if this is the active phase
    /// </summary>
    public void GivePlayerIFrames()
    {
        if (pc == null || !Active)
            return;
        pc.SetIFrames(dashIFrameDuration);
    }
}
