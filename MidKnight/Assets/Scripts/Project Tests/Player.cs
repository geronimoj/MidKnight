using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Player
    {
        [UnityTest]
        public IEnumerator Player_Move()
        {   //Get the player
            PlayerController player = CreatePlayer();
            //Move them
            player.Move(Vector3.right * 5 * Time.deltaTime);
            //Make sure they have moved
            Debug.Assert(player.transform.position != Vector3.zero);

            float timer = 0;
            //Continue moving them
            while (timer < 2)
            {
                player.Move(Vector3.right * 5 * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            //Check that they have moved the expected distance
            Debug.Assert(player.transform.position.x >= 10);
        }

        [UnityTest]
        public IEnumerator Player_Static_Assign()
        {
            Debug.Assert(PlayerController.Player == null);

            PlayerController player = CreatePlayer();
            yield return null;

            Debug.Assert(PlayerController.Player != null);
        }

        [UnityTest]
        public IEnumerator Player_Dash()
        {
            PlayerController player = CreatePlayer();
            player.transform.position = Vector3.zero;
            player.movement.Direction = Vector3.right;
            player.FacingRight = true;
            Dash dash = (Dash)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/States & Transitions/Dash.asset", typeof(Dash));
            Transition returnTrue = (Transition)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/States & Transitions/ReturnTrue.asset", typeof(Transition));
            returnTrue.target = dash;

            Debug.Assert(returnTrue != null);
            Debug.Assert(dash != null);
            StateManager sm = player.GetComponent<StateManager>();

            sm.globalTransitions = new Transition[1];
            sm.globalTransitions[0] = returnTrue;
            //Swap into the dash state and move for a bit
            yield return new WaitForSeconds(dash.duration);

            Debug.Assert(Approximate(player.transform.position.x, dash.distance, 0.5f));
        }

        private PlayerController CreatePlayer()
        {
            GameObject manager = GameObject.Instantiate(new GameObject());
            manager.tag = "GameManager";
            manager.AddComponent<GameManager>();
            GameObject ut = GameObject.Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
            GameObject child = GameObject.Instantiate(new GameObject(), ut.transform);
            child.AddComponent<Animator>();
            return ut.AddComponent<PlayerController>();
        }

        private bool Approximate(float f, float expected, float tolerant)
        {
            expected -= f;

            if (Mathf.Abs(expected) <= tolerant)
                return true;
            return false;
        }
    }
}
