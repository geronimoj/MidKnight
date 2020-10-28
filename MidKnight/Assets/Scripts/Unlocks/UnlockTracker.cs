using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTracker : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string, bool> unlocks = new Dictionary<string, bool>();
#if UNITY_EDITOR

#endif
    public void SetKey(string key, bool value)
    {
        foreach(KeyValuePair<string, bool> KVP in unlocks)
        {
            if (key == KVP.Key)
            {
                //unlocks.Remove(key);
                unlocks[key] = value;
                break;
            }
        }

        unlocks.Add(key, value);
    }
    public bool GetKeyValue(string key)
    {
        foreach(KeyValuePair<string, bool> KVP in unlocks)
        {
            if (key == KVP.Key)
            {
                return KVP.Value;
            }
        }

        return false;
    }
}
