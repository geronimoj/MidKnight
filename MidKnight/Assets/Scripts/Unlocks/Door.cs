using UnityEngine;

public class Door : MonoBehaviour
{
    public string switchNumber = "Switch 1";
    private UnlockTracker UT;

    public void Start()
    {
        UT = FindObjectOfType<UnlockTracker>();
        switchNumber = switchNumber.ToLower();
    }

    public void Update()
    {
        if (UT.GetKeyValue(switchNumber))
        {
            gameObject.SetActive(false);
        }
    }
}
