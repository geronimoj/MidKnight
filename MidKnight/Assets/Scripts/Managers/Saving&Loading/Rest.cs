using UnityEngine;

public class Rest : MonoBehaviour
{
    public int currentRestPoint = 0;
    private SavingManager SM;
    private EntitiesManager EM;

    private void Start()
    {
        SM = FindObjectOfType<SavingManager>();
        EM = FindObjectOfType<EntitiesManager>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                EM.EntitiesToNotRespawnUntillRest.Clear();

                foreach (Entities entity in EM.EntitiesToNeverRespawn)
                {
                    EM.EntitiesToNotRespawnUntillRest.Add(entity);
                }

                SM.currentRestPoint = currentRestPoint;
                other.GetComponent<CharacterController>().enabled = false;
                other.GetComponent<PlayerController>().TakeDamage(-other.GetComponent<PlayerController>().MaxHealth);
                other.GetComponent<PlayerController>().transform.position = new Vector3(other.GetComponent<PlayerController>().transform.position.x, other.GetComponent<PlayerController>().transform.position.y, other.GetComponent<PlayerController>().transform.position.z + 2);
                //Debug.Log("Save Text: " + SM.Save());
                Debug.Log("Save Binary: " + SM.Save(true));
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                other.GetComponent<CharacterController>().enabled = true;
                other.GetComponent<PlayerController>().transform.position = new Vector3(other.GetComponent<PlayerController>().transform.position.x, other.GetComponent<PlayerController>().transform.position.y, other.GetComponent<PlayerController>().transform.position.z - 2);
            }
        }
    }
}
