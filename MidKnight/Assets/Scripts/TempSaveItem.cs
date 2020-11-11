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
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (saving)
            {
                Debug.Log("Save: " + SM.SaveTxt(EM.EntitiesToNotRespawn, other.GetComponent<UnlockTracker>().unlocks));
            }
            else if (!saving)
            {
                Debug.Log("Load: " + SM.LoadTxt(ref EM.EntitiesToNotRespawn, ref other.GetComponent<UnlockTracker>().unlocks));
            }
        }
    }

}
