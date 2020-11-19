using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Unlocks
    {
        [Test]
        public void UnlockTracker_Set_Get_Values()
        {
            UnlockTracker ut = CreateUnlockTracker();
            //Check that require component is working
            Debug.Assert(ut.gameObject.GetComponent<PlayerController>() != null);
            //Set a key and check that its been added
            ut.SetKey("NewKey", false);
            Debug.Assert(ut.unlocks["NewKey"] == false);
            //Update the Keys value and check that a new key wasn't added
            ut.SetKey("NewKey", true);
            Debug.Assert(ut.unlocks.Count == 1);
            Debug.Assert(ut.unlocks["NewKey"] == true);
            //Add another Key to make sure it supports multiple keys
            ut.SetKey("AnotherKey", true);
            Debug.Assert(ut.unlocks.Count == 2);
            Debug.Assert(ut.unlocks["AnotherKey"] == true);
            //Test an invalid key
            Debug.Assert(ut.GetKeyValue("NotAKey") == false);
            //Test a valid key
            Debug.Assert(ut.GetKeyValue("NewKey") == true);
            //Check that the key is not case sensitive
            Debug.Assert(ut.GetKeyValue("AnOtherKeY") == true);
        }
        [UnityTest]
        public IEnumerator UnlocksWithEnumeratorPasses()
        {
            yield return null;
        }

        private UnlockTracker CreateUnlockTracker()
        {
            GameObject ut = GameObject.Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
            return ut.AddComponent<UnlockTracker>();
        }
    }
}
