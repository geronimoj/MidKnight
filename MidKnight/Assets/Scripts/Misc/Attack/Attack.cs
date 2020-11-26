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
            if (OnStart != null)
                OnStart.Invoke();
        }
        //Make sure deltaTime is consitent
        float f = Time.deltaTime;

        //Setting up storage locations for our returns and raycasts
        List<RaycastHit> hits = new List<RaycastHit>();
        RaycastHit[] hit;
        //Loop through the hitboxes and perform a raycast for each if it is active
        for (int i = 0; i < hitboxes.Length; i++)
        {   //Check if the hitbox was active this time period
            if (!GetHitBoxInfo(hitboxes[i], t, timer, f, out Vector3 origin, out Vector3 secOrigin, out Vector3 dir, out float dist))
                continue;
            //Make sure that dir and dist are non zero otherwise the raycast will return nothing
            //They are set to really small values so that it doesn't affect the raycast too much
            if (dir == Vector3.zero)
                dir.x = 1;
            if (dist == 0)
                dist = 0.001f;
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
            if (OnEnd != null)
                OnEnd.Invoke();
            complete = true;
            timer = 0;
        }
        else
            complete = false;

        return hits.ToArray();
    }
    /// <summary>
    /// Determines if the given hitbox was active during the time frame given & its positional & directional data
    /// </summary>
    /// <param name="h">The hitbox to check</param>
    /// <param name="t">The transform to use as its origin</param>
    /// <param name="timer">The current time</param>
    /// <param name="deltaTime">The change in time over this call</param>
    /// <param name="origin">Return the origin of the capsual hitbox</param>
    /// <param name="secOrigin">Return the origin of the other end of the capsual hitbox</param>
    /// <param name="dir">Return the direction of movement to its endpoint from startPoint</param>
    /// <param name="dist">Return the distance that will be moved along this frame</param>
    /// <returns>Returns true if the hitbox was active</returns>
    public bool GetHitBoxInfo(Hitbox h, Transform t, float timer, float deltaTime, out Vector3 origin, out Vector3 secOrigin, out Vector3 dir, out float dist)
    {
        origin = Vector3.zero;
        secOrigin = Vector3.zero;
        dir = Vector3.zero;
        dist = 0;

        float difInTime = timer - h.startTime;
        //Make sure this hitbox is still active
        //Is the current time below the start time?
        if (difInTime < 0
            //Will the current time + time.deltaTime step into the startTime?
            && difInTime + deltaTime < 0
            //Have we exceeded the endTime?
            || timer >= h.endTime)
            //The hitbox musn't be active so continue to the next one
            return false;
        //Repeat the check because I'm lazy.
        //This check determines if the change in time this frame would step into the raycasting time
        if (difInTime < 0)
        {   //Recalculate f to be shorter. Starting at startTime to where timer would end
            deltaTime = (timer + deltaTime) - h.startTime;
            //difInTime would be 0 since this simulates timer == startTime
            difInTime = 0;
        }
        //Check that the change in time would not exceed the endTime
        else if (timer + deltaTime > h.endTime)
            //If so recalculate the changeInTime to not exceed the endTime since we use it when calculating the distance of the raycast
            deltaTime = h.endTime - timer;

        //Calculate the position of origin, direction & distance.
        //Basically we create a vector. The x component of the start point goes along the right vector, z along forward & y along up
        origin = t.position + (t.right * h.startPoint.x) + (t.forward * h.startPoint.z) + (t.up * h.startPoint.y);
        //Calculate the direction of the raycast by repeating what we did on origin but for endPoint
        dir = t.position + (t.right * h.endPoint.x) + (t.forward * h.endPoint.z) + (t.up * h.endPoint.y);
        //Vector from A to B is B - A
        dir -= origin;
        //Move origin and sec along dir by how much time has passed as a percent
        origin += dir * (difInTime / (h.endTime - h.startTime));
        //Same as the first but we account for the orientation vector
        secOrigin = origin + (t.right * (h.orientation.normalized.x * h.length)) + (t.forward * (h.orientation.normalized.z * h.length)) + (t.up * (h.orientation.normalized.y * h.length));
        //Calculate the length of the raycast
        dist = dir.magnitude * deltaTime;

        return true;
    }
    /// <summary>
    /// Returns true if the hit gameobject was already hit.
    /// If the hit object is new, return false & add it to the targets hit
    /// </summary>
    /// <param name="h">The target to check</param>
    /// <returns>Returns false if it hasn't been hit and logs it so subsequent checks account for it having been hit</returns>
    private bool AlreadyHit(RaycastHit h)
    {   //Is the gameObject already hit
        if (targetsHit.Contains(h.transform.gameObject))
            return true;
        //Add it and return false
        targetsHit.Add(h.transform.gameObject);
        return false;
    }
}