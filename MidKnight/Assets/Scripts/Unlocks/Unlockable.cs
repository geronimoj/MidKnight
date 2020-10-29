using UnityEngine;

[RequireComponent(typeof(EntitiesManager))]
public class Unlockable : MonoBehaviour
{
    [SerializeField]
    private string[] powerUP = { "new moon" };
    [SerializeField]
    private EntitiesManager EM;

    private void Start()
    {
        EM = FindObjectOfType<EntitiesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Sets key you put in sets the value and adds the key if it doesn't exist
        foreach (string powerUP in powerUP)
        {
            other.gameObject.GetComponent<UnlockTracker>().SetKey(powerUP.ToLower(), true);
            LogEntity();
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
