using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests
{
    public class PhaseManager_test
    {
        [UnityTest]
        public IEnumerator PhaseManager_SwapCooldown()
        {
            PhaseManager pm = CreatePhaseManager();
            PlayerController pc = pm.GetComponent<PlayerController>();

            pm.SwapPhase(pm.everyMoonPhase[0], ref pc);
            Debug.Assert(pm.CurrentPhase == pm.everyMoonPhase[0]);

            yield return null;

            pm.SwapPhase(pm.everyMoonPhase[1], ref pc);
            Debug.Assert(pm.CurrentPhase == pm.everyMoonPhase[0]);
        }

        public static PhaseManager CreatePhaseManager()
        {
            PlayerController g = Player.CreatePlayer();
            PhaseManager pm = g.GetComponent<PhaseManager>();

            pm.swapCooldown = 1;

            pm.everyMoonPhase = new MoonPhase[4];
            pm.everyMoonPhase[0] = (NewMoon)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/Phases & Attacks/NewMoon.asset", typeof(NewMoon));
            pm.everyMoonPhase[1] = (CrescentMoon)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/Phases & Attacks/Crescent.asset", typeof(CrescentMoon));
            pm.everyMoonPhase[2] = (HalfMoon)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/Phases & Attacks/HalfMoon.asset", typeof(HalfMoon));
            pm.everyMoonPhase[3] = (MoonPhase)AssetDatabase.LoadAssetAtPath("Assets/Scriptables/Phases & Attacks/FullMoon.asset", typeof(MoonPhase));

            Debug.Assert(pm.everyMoonPhase[0] != null);
            Debug.Assert(pm.everyMoonPhase[1] != null);
            Debug.Assert(pm.everyMoonPhase[2] != null);
            Debug.Assert(pm.everyMoonPhase[3] != null);

            return pm;
        }
    }
}
