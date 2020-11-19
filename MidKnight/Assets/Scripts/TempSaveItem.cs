using UnityEngine;

public class TempSaveItem : MonoBehaviour
{
    public bool saving = true;
    private Character player;
    private EntitiesManager EM;
    private SavingManager SM;

    private void Start()
    {
        player = FindObjectOfType<Character>();
        EM = FindObjectOfType<EntitiesManager>();
        SM = FindObjectOfType<SavingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EM.EntitiesToNotRespawnUntillRest.Clear();

            foreach (Entities entity in EM.EntitiesToNeverRespawn)
            {
                EM.EntitiesToNotRespawnUntillRest.Add(entity);
            }

            player.TakeDamage(-player.MaxHealth);

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
