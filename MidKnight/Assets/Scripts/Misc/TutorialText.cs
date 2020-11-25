using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    /// <summary>
    /// Should the tutorialText fade in or out
    /// </summary>
    private bool fadeIn;
    /// <summary>
    /// The time it takes to fade in our out
    /// </summary>
    public float fadeInOutTime = 1f;
    /// <summary>
    /// The timer for fading in/out
    /// </summary>
    private float fadeTimer = 0;
    /// <summary>
    /// A reference to the textMesh component
    /// </summary>
    private TextMesh textMesh;
    /// <summary>
    /// A reference to the box collider
    /// </summary>
    private BoxCollider col;
    /// <summary>
    /// A reference to the players transform
    /// </summary>
    private Transform playerTrans;

    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
        col = GetComponent<BoxCollider>();

        if (textMesh == null)
            Debug.LogError("TextMesh not found on: " + gameObject.name);

        if (col == null)
            Debug.LogError("BoxCollider not found on: " + gameObject.name);
        else
            col.isTrigger = true;
    }

    void Update()
    {   //Since OnTriggerExit doesn't work on character controllers, we need to do a bit of math
        ShouldFadeOut();
        //Are we fading in our out
        if (fadeIn)
            fadeTimer += Time.deltaTime;
        else
            fadeTimer -= Time.deltaTime;
        //Clamp the timer fpr aupacitey
        fadeTimer = Mathf.Clamp(fadeTimer, 0, fadeInOutTime);
        //Update the colour
        Color col = textMesh.color;
        col.a = fadeTimer / fadeInOutTime;
        textMesh.color = col;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeIn = true;
            playerTrans = other.transform;
        }
    }
    /// <summary>
    /// Sets fadeIn to be false if the player is too far away
    /// </summary>
    private void ShouldFadeOut()
    {   //Make sure we have a valid transform
        if (playerTrans == null)
            return;
        //Get the difference in position
        Vector3 change = (transform.position + col.center) - playerTrans.position;
        //Check the x change
        if (Mathf.Abs(change.x) > (col.size.x / 2))
            fadeIn = false;
        //Check the y change
        else if (Mathf.Abs(change.y) > (col.size.y / 2))
            fadeIn = false;
    }
}
