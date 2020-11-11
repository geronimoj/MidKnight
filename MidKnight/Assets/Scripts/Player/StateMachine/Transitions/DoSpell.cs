using UnityEngine;

[CreateAssetMenu(fileName = "DoSpell", menuName = "Transitions/DoSpell", order = 5)]
public class DoSpell : Transition
{
    /// <summary>
    /// The name of the axis that represents the input key
    /// </summary>
    [Tooltip("The name of the axis that represents the key for this spell")]
    public string axisNameForSpellInput = "Null";
    /// <summary>
    /// The cost of the spell
    /// </summary>
    private float spellCost = 0;
    /// <summary>
    /// Is true if there is a new input
    /// </summary>
    private bool newInput = true;
    public override bool ShouldTransition(ref PlayerController c)
    {   //Make sure the spell has been assigned an axis
        if (axisNameForSpellInput == "Null")
        {
            Debug.Log("UnAssigned axis for DoSpell transition");
            return false;
        }
        //Make sure target is something before we try to cast it
        if (target != null)
        {
            //Cast target to being a castSpell
            CastSpell cs = target as CastSpell;
            //If it isn't null, that means target is a typeof castspell meaning we can get the spell cost
            if (cs != null)
                spellCost = cs.spellCost;
            else
                spellCost = 0;
        }
        //Make sure we have a new input, we have the moonlight for the spell & we have an input
        if (newInput && c.MoonLight >= spellCost && !c.Attacking && Input.GetAxisRaw(axisNameForSpellInput) != 0)
        {   //We don't have a new input anymore
            newInput = false;
            Debug.Log("Cast Spell");
            return true;
        }
        //Check that the button has been released.
        if (Input.GetAxisRaw(axisNameForSpellInput) == 0)
            newInput = true;
        return false;
    }
}
