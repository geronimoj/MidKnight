using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Attack_Test
    {
        [UnityTest]
        public IEnumerator Attack_AttackRaycast()
        {
            Attack attack = (Attack)ScriptableObject.CreateInstance(typeof(Attack));
            attack.duration = 1f;
            attack.hitboxes = new Hitbox[1];
            attack.hitboxes[0].radius = 0.5f;
            attack.hitboxes[0].length = 0;
            attack.hitboxes[0].orientation = Vector3.right;
            attack.hitboxes[0].endTime = 1;
            attack.hitboxes[0].length = 1;

            float timer = 0;
            Transform position = new GameObject().transform;

            attack.DoAttack(ref timer, position);

            Debug.Assert(!attack.AttackFinished);

            while(!attack.AttackFinished)
            {
                attack.DoAttack(ref timer, position);
                yield return null;
            }

            Debug.Assert(attack.AttackFinished);

            timer = 0;

            GameObject g = new GameObject();
            g.AddComponent<BoxCollider>();

            attack.hitboxes[0].startPoint = new Vector3(0, 5, 0);
            bool hitsomething = false;

            while (!attack.AttackFinished)
            {
                if (hitsomething == false)
                    hitsomething = attack.DoAttack(ref timer, position).Length != 0;

                if (hitsomething)
                    Debug.Assert(attack.DoAttack(ref timer, position).Length == 0);

                yield return null;
            }

            ScriptableObject.DestroyImmediate(attack);
        }
    }
}
