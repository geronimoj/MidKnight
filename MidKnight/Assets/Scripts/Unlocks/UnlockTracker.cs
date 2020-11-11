using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTracker : MonoBehaviour
{
    //Dictionary holding all the unlocks
    public Dictionary<string, bool> unlocks = new Dictionary<string, bool>();

    #region UNITY_EDITOR
#if UNITY_EDITOR
    //Arrays to show powerups and their boolean state
    public string[] stringArray;
    public bool[] boolArray;
#endif
    #endregion

    //Set a certain key to true or false and if it doesn't exist adds it
    public void SetKey(string key, bool value)
    {
        foreach (KeyValuePair<string, bool> KVP in unlocks)
        {
            if (key == KVP.Key)
            {
                //unlocks.Remove(key);
                unlocks[key] = value;

                #region UNITY_EDITOR
#if UNITY_EDITOR
                stringArray = new string[0];
                stringArray = new string[unlocks.Count()];
                boolArray = new bool[0];
                boolArray = new bool[unlocks.Count()];

                int e = 0;

                foreach (KeyValuePair<string, bool> kvp in unlocks)
                {
                    stringArray[e] = kvp.Key;
                    boolArray[e] = kvp.Value;
                    e++;
                }
#endif
                #endregion

                return;
            }
        }

        unlocks.Add(key, value);

        #region UNITY_EDITOR
#if UNITY_EDITOR
        stringArray = new string[0];
        stringArray = new string[unlocks.Count()];
        boolArray = new bool[0];
        boolArray = new bool[unlocks.Count()];

        int i = 0;

        foreach (KeyValuePair<string, bool> KVP in unlocks)
        {
            stringArray[i] = KVP.Key;
            boolArray[i] = KVP.Value;
            i++;
        }
#endif
        #endregion
    }
    //Gets the value of a key
    public bool GetKeyValue(string key)
    {
        key = key.ToLower();
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
