using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StateManager_Test
    {
        [UnityTest]
        public IEnumerator StateManager_Start()
        {
            StateManager sm = CreateStateManager();

            yield return null;
        }

        private StateManager CreateStateManager()
        {
            GameObject g = GameObject.Instantiate(new GameObject());
            g.AddComponent<StateManager>();

            Debug.Assert(g.GetComponent<PlayerController>() != null);
            return g.GetComponent<StateManager>();
        }
    }
}
