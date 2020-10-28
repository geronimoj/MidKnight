using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTracker : MonoBehaviour
{
    Dictionary<string, bool> unlocks = new Dictionary<string, bool>();
#if UNITY_EDITOR
    public string[] stringArray;
    public bool[] boolArray;
#endif
    public void SetKey(string key, bool value)
    {
        foreach(KeyValuePair<string, bool> KVP in unlocks)
        {
            if (key == KVP.Key)
            {
                //unlocks.Remove(key);
                unlocks[key] = value;
#if UNITY_EDITOR
                int e = 0;

                foreach (KeyValuePair<string, bool> kvp in unlocks)
                {
                    if (key == kvp.Key)
                    {
                        boolArray[e] = value;
                        break;
                    }

                    e++;
                }
#endif
                return;
            }
        }

        unlocks.Add(key, value);
#if UNITY_EDITOR
        stringArray = new string[unlocks.Count()];
        boolArray = new bool[unlocks.Count()];

        int i = 0;

        foreach (KeyValuePair<string, bool> KVP in unlocks)
        {
            stringArray[i] = KVP.Key;
            boolArray[i] = KVP.Value;
            i++;
        }
#endif
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
