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
            Debug.Assert(ut.gameObject.GetComponent<UnlockTracker>() != null);
            //Set a key and check that its been added
            ut.SetKey("NewKey", false);
            Debug.Assert(ut.unlocks["newkey"] == false);
            //Update the Keys value and check that a new key wasn't added
            ut.SetKey("NewKey", true);
            Debug.Assert(ut.unlocks.Count == 1);
            Debug.Assert(ut.unlocks["newkey"] == true);
            //Add another Key to make sure it supports multiple keys
            ut.SetKey("AnotherKey", true);
            Debug.Assert(ut.unlocks.Count == 2);
            Debug.Assert(ut.unlocks["anotherkey"] == true);
            //Test an invalid key
            Debug.Assert(ut.GetKeyValue("NotAKey") == false);
            //Test a valid key
            Debug.Assert(ut.GetKeyValue("NewKey") == true);
            //Check that the key is not case sensitive
            Debug.Assert(ut.GetKeyValue("AnOtherKeY") == true);
        }

        [Test]
        public void UnlockTracker_Check_Health_And_Eclipse()
        {
            UnlockTracker ut = CreateUnlockTracker();

            Debug.Assert(ut.HealthAdd == 0);
            //We can't explicitly call CheckHealth so we do it through SetKey
            ut.SetKey("health", true);
            Debug.Assert(ut.HealthAdd == 1);
            ut.SetKey("health", true);
            Debug.Assert(ut.HealthAdd == 2);
            //Set the players health stat
            PlayerController pc = ut.GetComponent<PlayerController>();
            //Increment health again. It shold reset to 0 and give the player max health
            ut.SetKey("health", true);
            Debug.Assert(ut.HealthAdd == 0);
            Debug.Assert(pc.MaxHealth == 2);
            Debug.Assert(pc.Health == 2);
        }


        private UnlockTracker CreateUnlockTracker()
        {
            GameObject manager = GameObject.Instantiate(new GameObject());
            manager.tag = "GameManager";
            manager.AddComponent<GameManager>();
            GameObject ut = GameObject.Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
            GameObject child = GameObject.Instantiate(new GameObject(), ut.transform);
            child.AddComponent<Animator>();
            ut.AddComponent<PlayerController>();
            return ut.GetComponent<UnlockTracker>();
        }
    }
}
