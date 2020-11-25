using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    /// <summary>
    /// The storage location for the screenfade reference
    /// </summary>
    private static ScreenFade screenFader;
    /// <summary>
    /// A Get for screen fader
    /// </summary>
    public static ScreenFade ScreenFader
    {
        get
        {
            return screenFader;
        }
    }
    /// <summary>
    /// How long the fade takes to happen
    /// </summary>
    public float fadeTime = 1;
    /// <summary>
    /// A timer for the screen fade
    /// </summary>
    private float fadeTimer = 0;
    /// <summary>
    /// Whether we should fade in or out
    /// </summary>
    private bool fadeIn = false;
    /// <summary>
    /// A reference to the MeshRenderer
    /// </summary>
    private Image i;

    private void Start()
    {
        screenFader = this;
        i = GetComponent<Image>();
        if (i == null)
            Debug.LogError("MeshRenderer not on GameObject: " + gameObject.name);
    }

    private void Update()
    {
        if (fadeIn)
            fadeTimer += Time.deltaTime;
        else
            fadeTimer -= Time.deltaTime;

        fadeTimer = Mathf.Clamp(fadeTimer, 0, fadeTime);

        Color col = i.color;
        col.a = fadeTimer / fadeTime;
        i.color = col;
    }
    /// <summary>
    /// Returns true when the transition has finished fading either in or out
    /// </summary>
    /// <returns>Returns true when the fading is complete</returns>
    public bool FadeFinished()
    {
        if (fadeIn && fadeTimer >= fadeTime)
            return true;
        else if (!fadeIn && fadeTimer <= 0)
            return true;
        return false;
    }
    /// <summary>
    /// Call to begin fading in
    /// </summary>
    public void FadeIn()
    {
        fadeIn = true;
    }
    /// <summary>
    /// Call to begin fading out
    /// </summary>
    public void FadeOut()
    {
        fadeIn = false;
    }
}
