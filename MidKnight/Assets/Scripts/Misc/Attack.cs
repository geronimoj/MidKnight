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
        if (timer <= 0)
        {
            complete = false;
            targetsHit.Clear();
            OnStart.Invoke();
        }
        //Make sure deltaTime is consitent
        float f = Time.deltaTime;

        //Setting up storage locations for our returns and raycasts
        List<RaycastHit> hits = new List<RaycastHit>();
        RaycastHit[] hit;
        Vector3 origin;
        Vector3 secOrigin;
        Vector3 dir;
        float dist;
        //Loop through the hitboxes and perform a raycast for each if it is active
        for (int i = 0; i < hitboxes.Length; i++)
        {
            float difInTime = timer - hitboxes[i].startTime;
            float fTemp = f;
            //Make sure this hitbox is still active
                //Is the current time below the start time?
            if (difInTime < 0
                //Will the current time + time.deltaTime step into the startTime?
                && difInTime + f < 0
                //Have we exceeded the endTime?
                && timer >= hitboxes[i].endTime)
                //The hitbox musn't be active so continue to the next one
                continue;
            //Repeat the check because I'm lazy.
            //This check determines if the change in time this frame would step into the raycasting time
            if (difInTime < 0)
            {   //Recalculate f to be shorter. Starting at startTime to where timer would end
                fTemp = (timer + f) - hitboxes[i].startTime;
                //difInTime would be 0 since this simulates timer == startTime
                difInTime = 0;
            }
            //Check that the change in time would not exceed the endTime
            else if (timer + f > hitboxes[i].endTime)
                //If so recalculate the changeInTime to not exceed the endTime since we use it when calculating the distance of the raycast
                fTemp = hitboxes[i].endTime - timer;

            //Calculate the position of origin, direction & distance.
            //Basically we create a vector. The x component of the start point goes along the right vector, z along forward & y along up
            origin = t.position + (t.right * hitboxes[i].startPoint.x) + (t.forward * hitboxes[i].startPoint.z) + (t.up * hitboxes[i].startPoint.y);
            //Same as the first but we account for the orientation vector
            secOrigin = origin + (t.right * (hitboxes[i].orientation.normalized.x * hitboxes[i].length)) + (t.forward * (hitboxes[i].orientation.normalized.z * hitboxes[i].length)) + (t.up * (hitboxes[i].orientation.normalized.y * hitboxes[i].length));
            //Calculate the direction of the raycast by repeating what we did on origin but for endPoint
            dir = t.position + (t.right * hitboxes[i].endPoint.x) + (t.forward * hitboxes[i].endPoint.z) + (t.up * hitboxes[i].endPoint.y);
            //Vector from A to B is B - A
            dir -= origin;
            //Move origin and sec along dir by how much time has passed as a percent
            origin += dir * (difInTime / (hitboxes[i].endTime - hitboxes[i].startTime));
            //Calculate the length of the raycast
            dist = dir.magnitude * fTemp;
            //Perform the raycast for this hitbox
            hit = Physics.CapsuleCastAll(origin, secOrigin, hitboxes[i].radius, dir.normalized, dist);
            //Make sure we hit something
            if (hit != null)
                //Check if the hit objects have already been hit, if not, add them to the return
                foreach (RaycastHit h in hit)
                    //If already hit returns false, the RaycastHit will be automatically added to targetsHit but we need to add it to the hits array
                    if (!AlreadyHit(h))
                        hits.Add(h);
        }

        //Increment the timer
        timer += f;
        //Has the attack finished?
        if (timer >= duration)
        {
            OnEnd.Invoke();
            complete = true;
            timer = 0;
        }
        else
            complete = false;

        return hits.ToArray();
    }

    private bool AlreadyHit(RaycastHit h)
    {   //Is the gameObject already hit
        if (targetsHit.Contains(h.transform.gameObject))
            return true;
        //Add it and return false
        targetsHit.Add(h.transform.gameObject);
        return false;
    }
}