using UnityEngine;

public class TempSaveItem : MonoBehaviour
{
    public bool saving = true;
    private EntitiesManager EM;
    private SavingManager SM;

    private void Start()
    {
        EM = FindObjectOfType<GameManager>().GetComponent<EntitiesManager>();
        SM = FindObjectOfType<GameManager>().GetComponent<SavingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (saving)
            {
                Debug.Log("Save Text: " + SM.Save(false, EM.EntitiesToNotRespawn, other.GetComponent<UnlockTracker>().unlocks));
                Debug.Log("Save Binary: " + SM.Save(true, EM.EntitiesToNotRespawn, other.GetComponent<UnlockTracker>().unlocks));
            }
            else if (!saving)
            {
                Debug.Log("Load Text: " + SM.Load(false, ref EM.EntitiesToNotRespawn, ref other.GetComponent<UnlockTracker>().unlocks));
                Debug.Log("Load Binary: " + SM.Load(true, ref EM.EntitiesToNotRespawn, ref other.GetComponent<UnlockTracker>().unlocks));
            }
        }
    }
}
