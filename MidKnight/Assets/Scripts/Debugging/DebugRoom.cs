using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRoom : MonoBehaviour
{
    public Room room;
    public float pointRadius;
#if !UNITY_EDITOR
    private void Awake()
    {
        Destroy(this);
    }
#endif

    private void OnDrawGizmos()
    {
        if (room == null)
            return;
        for (int i = 0; i < room.pathNodes.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PNodeToV3(room.pathNodes[i]), pointRadius);

            if (i < room.pathNodes.Length - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(PNodeToV3(room.pathNodes[i]), PNodeToV3(room.pathNodes[i + 1]));
            }
        }
    }

    private Vector3 PNodeToV3 (Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }
}
