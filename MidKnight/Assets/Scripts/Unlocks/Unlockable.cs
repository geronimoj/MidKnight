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
                other.gameObject.GetComponent<UnlockTracker>().SetKey("new moon", true);
                Destroy(gameObject);
                break;
            case "crescent moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("crescent moon", true);
                Destroy(gameObject);
                break;
            case "half moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("half moon", true);
                Destroy(gameObject);
                break;
            case "dash":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("dash", true);
                Destroy(gameObject);
                break;
            case "full moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("full moon", true);
                Destroy(gameObject);
                break;
            case "double jump":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("double jump", true);
                Destroy(gameObject);
                break;
            case "moon beam":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("moon beam", true);
                Destroy(gameObject);
                break;
            case "eclipse":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("eclipse", true);
                Destroy(gameObject);
                break;
        }
    }
}
