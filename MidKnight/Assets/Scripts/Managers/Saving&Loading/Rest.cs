using UnityEngine;

public class Rest : MonoBehaviour
{
    public int currentRestPoint = 0;
    private SavingManager SM;
    private bool inRest = false;

    private void Start()
    {
        SM = FindObjectOfType<SavingManager>();
        inRest = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!inRest && Input.GetAxis("Vertical") > 0)
            {
                SM.currentRestPoint = currentRestPoint;
                inRest = true;
                SM.EnterRestPoint();
                other.GetComponent<CharacterController>().enabled = false;
            }
            else if (inRest && Input.GetAxis("Vertical") < 0)
            {
                other.GetComponent<CharacterController>().enabled = true;
            }
        }
    }
}
