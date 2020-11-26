using UnityEngine;

public class Unlockable : MonoBehaviour
{
    [SerializeField]
    protected string[] powerUP = { "new moon" };
    [SerializeField]
    private EntitiesManager EM;

    protected void Start()
    {
        EM = FindObjectOfType<EntitiesManager>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        //Make sure the player is the one that touched it
        if (other.CompareTag("Player"))
        {
            bool loged = false;
            //Sets key you put in sets the value and adds the key if it doesn't exist
            foreach (string powerUP in powerUP)
            {
                other.gameObject.GetComponent<UnlockTracker>().SetKey(powerUP.ToLower(), true);

                if (!loged)
                {
                    LogEntity();
                    loged = true;
                }
            }
        }
    }
    
    private void LogEntity()
    {
        Entities e;
        Room R = GetComponentInParent<Room>();
        e.thisRoom = R.roomID;
        e.index = -1;

        for (int i = 0; i < R.NonRespawningRoomObjects.Count; i++)
        {
            if (R.NonRespawningRoomObjects[i] == gameObject)
            {
                e.index = i;
                break;
            }
        }

        EM.EntitiesToNeverRespawn.Add(new Entities(e.index, R.roomID)); //{ thisRoom = R.roomID, index = e.index });
        EM.EntitiesToNotRespawnUntillRest.Add(new Entities(e.index, R.roomID)); //{ thisRoom = R.roomID, index = e.index });
        gameObject.SetActive(false);
    }
}
