using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScreenFader_Test
    {
        [UnityTest]
        public IEnumerator ScreenFader_Static_Assign()
        {   //Check that the static value is null
            Debug.Assert(ScreenFade.ScreenFader == null);
            //Create the screenfader
            CreateScreenFader();
            //Wait a frame
            yield return null;
            //Check that it is no-longer null
            Debug.Assert(ScreenFade.ScreenFader != null);
        }

        [UnityTest]
        public IEnumerator ScreenFader_Fade_In_Out()
        {
            ScreenFade sf = CreateScreenFader();

            sf.FadeIn();

            yield return new WaitForSeconds(sf.fadeTime / 2);

            Debug.Assert(!sf.FadeFinished());

            yield return new WaitForSeconds(sf.fadeTime / 2);

            Debug.Assert(sf.FadeFinished());

            sf.FadeOut();
            yield return new WaitForSeconds(sf.fadeTime);
            Debug.Assert(sf.FadeFinished());
        }

        public ScreenFade CreateScreenFader()
        {
            //Create the screen fader
            GameObject g = GameObject.Instantiate(new GameObject());
            g.AddComponent<ScreenFade>();
            //Check that the image was assigned
            Debug.Assert(g.GetComponent<Image>() != null);
            //Return the gameObject
            return g.GetComponent<ScreenFade>();
        }
    }
}
