using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDebugger : MonoBehaviour
{
    public Room roomToDisplay;

    private void Start()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
