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
                Debug.Log("Save Text: " + SM.Save(false));
                Debug.Log("Save Binary: " + SM.Save(true));
            }
            else if (!saving)
            {
                Debug.Log("Load Text: " + SM.Load(false));
                Debug.Log("Load Binary: " + SM.Load(true));
            }
        }
    }
}
