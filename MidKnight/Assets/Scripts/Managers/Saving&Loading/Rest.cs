using UnityEngine;

public class Rest : MonoBehaviour
{
    public int thisRestPoint = 0;
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
                SM.currentRestPoint = thisRestPoint;
                inRest = true;
                SM.EnterRestPoint();
            }
            else if (inRest && Input.GetAxis("Vertical") < 0)
            {
                other.GetComponent<PlayerController>().enabled = true;
                other.GetComponent<PlayerController>().animator.SetTrigger("ExitRest");
                inRest = false;
            }
        }
    }
}
