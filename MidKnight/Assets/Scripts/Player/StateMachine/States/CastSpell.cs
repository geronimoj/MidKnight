using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty Spell", menuName = "States/EmptySpell", order = 3)]
public class CastSpell : State
{
    /// <summary>
    /// The time before we cast the spell
    /// </summary>
    [Range(0,100)]
    public float windUp = 0;
    /// <summary>
    /// The time after we finish the spell before we switch states
    /// </summary>
    [Range(0, 100)]
    public float endLag = 0;
    /// <summary>
    /// windUp Timer
    /// </summary>
    private float windUpTimer = 0;
    /// <summary>
    /// endLag Timer
    /// </summary>
    private float endLagTimer = 0;
    /// <summary>
    /// The cost of the spell
    /// </summary>
    public float spellCost = 0;

    private bool spellCasted = false;

    public override void StateStart(ref PlayerController c)
    {   //Setup the timers
        windUpTimer = windUp;
        endLagTimer = endLag;
        //Set us to not have cast the spell yet
        spellCasted = false;
        //Make sure there is a slot to ignore the ongroundelseairborne transition
        if (ignoreTransitions.Length == 0)
            ignoreTransitions = new bool[1];
        ignoreTransitions[0] = true;
        //Call anything extra the player wants to do
        ExtraOnEnter(ref c);
    }

    public override void StateUpdate(ref PlayerController c)
    {   //If we have windUp to do, dcrement the timer
        if (windUpTimer >= 0)
            windUpTimer -= Time.deltaTime;
        else
            //Otherwise decrement the endLag
            endLagTimer -= Time.deltaTime;
        ExtraUpdate(ref c);
        //If the windUp is done, cast the spell
        if (windUpTimer <= 0 && !spellCasted)
        {   //Don't recast the spell
            spellCasted = true;
            DoSpell(ref c);
        }
        //If endLag is done, re-enable to ongroundelseairborne transition
        else if (endLagTimer < 0)
            ignoreTransitions[0] = false;
    }
    /// <summary>
    /// An overrideable function that will be called once, when windUp Timer has reached 0
    /// </summary>
    /// <param name="c">A reference to the player controller</param>
    protected virtual void DoSpell(ref PlayerController c)
    {
        Debug.Log("Empty Spell");
    }

    protected virtual void ExtraUpdate(ref PlayerController c)
    {

    }

    protected virtual void ExtraOnEnter(ref PlayerController c)
    {

    }
}
