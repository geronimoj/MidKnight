using UnityEngine;

[RequireComponent(typeof(EntitiesManager))]
public class Unlockable : MonoBehaviour
{
    [SerializeField]
    private string powerUP = "new moon";
    [SerializeField]
    private EntitiesManager EM;

    private void Start()
    {
        EM = FindObjectOfType<EntitiesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Depending on which key you put in sets the value and adds the key if it doesn't exist
        switch(powerUP)
        {
            case "new moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("new moon", true);
                LogEntity();
                break;
            case "crescent moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("crescent moon", true);
                LogEntity();
                break;
            case "half moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("half moon", true);
                LogEntity();
                break;
            case "dash":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("dash", true);
                LogEntity();
                break;
            case "full moon":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("full moon", true);
                LogEntity();
                break;
            case "double jump":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("double jump", true);
                LogEntity();
                break;
            case "moon beam":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("moon beam", true);
                LogEntity();
                break;
            case "eclipse":
                other.gameObject.GetComponent<UnlockTracker>().SetKey("eclipse", true);
                LogEntity();
                break;
        }
    }
    
    private void LogEntity()
    {
        Entities e;
        Room R = GetComponentInParent<Room>();
        e.thisRoom = R.roomID;
        e.index = -1;

        for (int i = 0; i < R.roomObjects.Count; i++)
        {
            if (R.roomObjects[i] == gameObject)
            {
                e.index = i;
                break;
            }
        }

        EM.EntitiesToNotRespawn.Add(e);
        gameObject.SetActive(false);
    }
}
