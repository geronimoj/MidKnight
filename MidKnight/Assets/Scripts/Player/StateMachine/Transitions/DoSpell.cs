using UnityEngine;

[CreateAssetMenu(fileName = "DoSpell", menuName = "Transitions/DoSpell", order = 5)]
public class DoSpell : Transition
{
    /// <summary>
    /// The name of the axis that represents the input key
    /// </summary>
    [Tooltip("The name of the axis that represents the key for this spell")]
    public string axisNameForSpellInput = "Cancel";
    /// <summary>
    /// The cost of the spell
    /// </summary>
    public float spellCost = 0;
    /// <summary>
    /// Is true if there is a new input
    /// </summary>
    private bool newInput = true;
    public override bool ShouldTransition(ref PlayerController c)
    {
        if (newInput && c.MoonLight >= spellCost && Input.GetAxisRaw(axisNameForSpellInput) != 0)
        {
            newInput = false;
            Debug.Log("Cast Spell");
            return true;
        }

        if (Input.GetAxisRaw(axisNameForSpellInput) == 0)
            newInput = true;
        return false;
    }
}
