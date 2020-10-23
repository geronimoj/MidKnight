using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    public string powerUP = "new moon";

    private void OnTriggerEnter(Collider other)
    {
        switch(powerUP)
        {
            case "new moon":
                other.gameObject.GetComponent<UnlockTracker>().AddNewMoon();
                Destroy(gameObject);
                break;
            case "crescent moon":
                other.gameObject.GetComponent<UnlockTracker>().AddCrescentMoon();
                Destroy(gameObject);
                Debug.Log("crescent moon");
                break;
            case "half moon":
                other.gameObject.GetComponent<UnlockTracker>().AddHalfMoon();
                Destroy(gameObject);
                break;
            case "dash":
                other.gameObject.GetComponent<UnlockTracker>().AddDash();
                Destroy(gameObject);
                break;
            case "full moon":
                other.gameObject.GetComponent<UnlockTracker>().AddFullMoon();
                Destroy(gameObject);
                break;
            case "double jump":
                other.gameObject.GetComponent<UnlockTracker>().AddDoubleJump();
                Destroy(gameObject);
                break;
            case "moon beam":
                other.gameObject.GetComponent<UnlockTracker>().AddMoonBeam();
                Destroy(gameObject);
                break;
            case "eclipse":
                other.gameObject.GetComponent<UnlockTracker>().AddEclipse();
                Destroy(gameObject);
                break;
        }
    }
}
