using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Attack", menuName = "Attack", order = 5)]
public class Attack : ScriptableObject
{
    /// <summary>
    /// The duration of the attack
    /// </summary>
    [Range(0, 100)]
    public float duration = 0;
    /// <summary>
    /// A storage location for enemies that were hit by attacks so they don't take damage multiple times
    /// </summary>
    protected List<GameObject> targetsHit = new List<GameObject>();

    public Hitbox[] hitboxes;
    /// <summary>
    /// Is set to whether the attack has finished or not
    /// </summary>
    private bool complete = false;
    /// <summary>
    /// Returns true once the attack has finished
    /// </summary>
    public bool AttackFinished
    {
        get
        {
            return complete;
        }
    }
    /// <summary>
    /// Called when the attack begins
    /// </summary>
    public UnityEvent OnStart;
    /// <summary>
    /// Called when the attack ends
    /// </summary>
    public UnityEvent OnEnd;
    /// <summary>
    /// Performs a raycast for the attack this frame. Logs the hit gameObjects.
    /// Will not include objects hit by previous raycasts from this attack.
    /// Hit objects are cleared when the timer is reset to 0
    /// </summary>
    /// <param name="timer">A reference to a timer</param>
    /// <param name="t">The position & direction of the attack</param>
    /// <returns>Returns any new objects hit by the raycast. Objects hit is reset when the timer is 0</returns>
    public RaycastHit[] DoAttack(ref float timer, Transform t)
    {
        if (timer == 0)
        {
            targetsHit.Clear();
            OnStart.Invoke();
        }
        timer += Time.deltaTime;
        //Has the attack finished?
        if (timer >= duration)
        {
            OnEnd.Invoke();
            complete = true;
            timer = 0;
        }
        else
            complete = false;

        return null;
    }
}