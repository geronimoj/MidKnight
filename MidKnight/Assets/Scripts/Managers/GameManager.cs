﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Room room;
    /// <summary>
    /// Snaps the position given onto the path
    /// INCOMPLETE
    /// </summary>
    /// <param name="position">The current position of the object</param>
    /// <returns>The new position of the object</returns>
    public Vector3 SnapToPath(Vector3 position)
    {   //Make sure we have a room with enough nodes to create a path
        if (room == null || room.pathNodes.Length < 2)
        {
            position.z = 0;
            return position;
        }
        //We get the best segment
        int bestSegment = GetSegmentIndex(position);

        //Store the segments vector so we don't have to keep recalculating it
        Vector2 bestSegVec = room.pathNodes[bestSegment + 1] - room.pathNodes[bestSegment];

        //Store the dot product. This is just so we don't keep asking for more floats. We are currently using it for calulating the overshoot for this segment
        float dot = Vector2.Dot(V3ToNode(position) - room.pathNodes[bestSegment], bestSegVec);
        //Store the overshoot
        float bestSegOverShoot;
        //Assign overshoot the overshoot value of the bestSegment
        if (dot < 0)
            bestSegOverShoot = Mathf.Abs(dot);
        else
            bestSegOverShoot = dot - bestSegVec.magnitude;
        //Store the distance from the start of the segment for the best segment
        Vector2 distanceToPos = V3ToNode(position) - room.pathNodes[bestSegment];

        //We should now have the "best segment" to move the position onto
        //We store the movementDistance because if we overshoot, we want to manipulate this variable
        float moveDist = Vector2.Dot(distanceToPos, bestSegVec);
        //If we overshoot, we need to adjust out bestSegment & bestSegVec
        if (bestSegOverShoot > 0)
        {   //MoveDist is the Dot so we can use it to check if we overshot to the left or right
            if (moveDist < 0)
            {   //We are moving into the previous segment 
                bestSegment--;
                //Make sure moveDist is positive since we will be inverting the movement vectors
                moveDist = Mathf.Abs(moveDist);
                //Make sure we don't hit out of range exceptions with edge cases, literally
                if (bestSegment < 0)
                {   //Invert this segments vector and use it instead
                    bestSegVec = -bestSegVec;
                    bestSegment = 0;
                    //We don't need to re-calculate distanceToPos since bestSegment had to be 0 initially to even reach this edge case
                }
                else
                    //We are still in range so we need to calculate the new bestSegVec
                    //We do it backwards so we are going right to left instead of left to right like we have been doing this entire time
                    //Saves us having to invert it later
                    bestSegVec = room.pathNodes[bestSegment] - room.pathNodes[bestSegment + 1];
                    //We don't have to recalculate distanceToPos since we are still using the same starting node
            }
            else
            {   //We are moving into the next segment
                bestSegment++;
                //Make sure we don't hit out of range exceptions with edge cases, literally
                if (bestSegment >= room.pathNodes.Length - 1)
                   //We are out of range so undo our changes to bestSegment
                    bestSegment--;
                    //We don't need to invert bestSegVec since we are still moving right (dot was positive)
                    //We literally do nothing else since we are basically just moving further along this current segment than usual to avoid the outOfRange stuff
                else
                {   //Subtract the magnitude from moveDist to simulate moving along the segment
                    moveDist -= bestSegVec.magnitude;
                    //Recalculate bestSegVec since we incremented bestSegment meaning we need to update the vector
                    bestSegVec = room.pathNodes[bestSegment + 1] - room.pathNodes[bestSegment];
                }
            }
        }
        //Calculate where the position give would be on the x & z axis by moving along bestSegVec from the index of bestSegment
        Vector2 v = room.pathNodes[bestSegment] + (bestSegVec.normalized * moveDist);
        //Assign positions x & z values to what was just calculated by keep the y position
        return new Vector3(v.x, position.y, v.y);
    }
    /// <summary>
    /// Returns a Vector2 using a Vector3's x & z component
    /// </summary>
    /// <param name="v">The vector to convert</param>
    /// <returns>Returns the top down view of the position as a vector 2</returns>
    private Vector2 V3ToNode(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    /// <summary>
    /// Returns the index to the segment the 3D cords would map onto
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private int GetSegmentIndex(Vector3 position)
    {
        if (room == null || room.pathNodes.Length < 2)
            return -1;
        //Store the bestSegment
        int bestSegment = 0;
        //Store the dot product. This is just so we don't keep asking for more floats. We are currently using it for calulating the overshoot for this segment
        float dot = Vector2.Dot(room.pathNodes[bestSegment + 1] - room.pathNodes[bestSegment], V3ToNode(position) - room.pathNodes[bestSegment]);
        //Store the overshoot
        float bestSegOverShoot;
        //Assign overshoot the overshoot value of the bestSegment
        if (dot < 0)
            bestSegOverShoot = Mathf.Abs(dot);
        else
            bestSegOverShoot = dot - (room.pathNodes[bestSegment + 1] - room.pathNodes[bestSegment]).magnitude;
        //Store the distance from the start of the segment for the best segment
        Vector2 distanceToPos = V3ToNode(position) - room.pathNodes[bestSegment];
        //A storage location for the segment so we don't have to keep recalculating it
        Vector2 segment;
        float overShoot;
        bool replaceSegment;

        for (int i = 1; i < room.pathNodes.Length - 1; i++)
        {   //Make sure we have to pass the checks to replace the segment
            replaceSegment = false;
            //Get & store the info needed
            segment = room.pathNodes[i + 1] - room.pathNodes[i];
            dot = Vector3.Dot(V3ToNode(position) - room.pathNodes[i], segment);
            //calculate the overShoot
            if (dot < 0)
                overShoot = Mathf.Abs(dot);
            else
                overShoot = dot - segment.magnitude;

            //Does position project inside or outside of the current segment
            if (overShoot > 0)
            {   //If it projects outside, by how much does it "overshoot" this segment
                //Is the "overshoot" < than the bestSegments "overshoot". (Overshoot will be negative if the dot does not overshoot)
                //Overshoot should always be positive unless it did not overshoot
                if (overShoot < bestSegOverShoot)
                    //If so, make this segment the best segment.
                    replaceSegment = true;
                //otherwise just continue
            }
            //We didn't overshoot
            else
            {
                //If we don't overshoot this segment, did the bestSegment overshoot?
                if (bestSegOverShoot > 0)
                    //If so, replace bestSegment with us.
                    replaceSegment = true;
                //If not, neither segments overshoot. So compare the distance to position, lowest distance is written to bestSegment
                else if ((V3ToNode(position) - room.pathNodes[i]).magnitude < distanceToPos.magnitude)
                    replaceSegment = true;
            }

            if (replaceSegment)
            {   //If so, make this segment the best segment.
                bestSegOverShoot = overShoot;
                bestSegment = i;
                distanceToPos = V3ToNode(position) - room.pathNodes[i];
            }
        }

        return bestSegment;
    }
    /// <summary>
    /// Returns the direction of the path to the right
    /// </summary>
    /// <param name="position">The position of the player</param>
    /// <returns>A unit vector pointing along the path to the right along the horizontal plane</returns>
    public Vector3 GetPathDirectionRight(Vector3 position)
    {
        if (room == null || room.pathNodes.Length < 2)
            return Vector3.right;
        //Get the index of the segment we are in
        int segIndex = GetSegmentIndex(position);
        //Calculate the direction of the segment
        Vector2 dir = room.pathNodes[segIndex + 1] - room.pathNodes[segIndex];
        //Normalise it
        dir.Normalize();
        //Return it as a Vector3. (The y axis is up and down, we don't care about it)
        return new Vector3(dir.x, 0, dir.y);
    }
    /// <summary>
    /// Gets the path for readonly purposes
    /// </summary>
    /// <returns>An array of the nodes of the path</returns>
    public Vector2[] GetPath()
    {
        if (room != null && room.pathNodes != null)
            return room.pathNodes;
        return null;
    }
    /// <summary>
    /// Takes the position and movement vector and returns a new movement vector that will end the character along the path
    /// </summary>
    /// <param name="position">The position of the character</param>
    /// <param name="moveVector">The movement vector of the character</param>
    /// <returns>The new movement vector</returns>
    public Vector3 MoveAlongPath(Vector3 position, Vector3 moveVector)
    {
        if (room == null || room.pathNodes.Length < 2)
        {
            moveVector.z = 0;
            return moveVector;
        }
        Vector3 pos = SnapToPath(position);

        int curSeg = GetSegmentIndex(pos);

        Vector2 segVec = room.pathNodes[curSeg + 1] - room.pathNodes[curSeg];
        Vector2 distIntoSeg = V3ToNode(pos) - room.pathNodes[curSeg];

        Vector3 point = pos;
        
        //Are we moving left or right?
        if (Vector3.Dot(segVec, moveVector) < 0)
        {   //We are moving left
            //Will we hit the end of this segment?
            if (distIntoSeg.magnitude - moveVector.magnitude < 0)
            {   //We are moving into the next segment
                curSeg--;
                //Check for out of range errors
                if (curSeg < 0)
                {   //Out of range so just use the segment at 0, which, surprisingly, is currently segVec
                    Vector2 dirWMag = (-segVec.normalized) * moveVector.magnitude;
                    point = pos + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
                }
                else
                {   //Not out of range so calculate the previous segment
                    //Calculate the distance we need to move down the next segment
                    float extraDist = distIntoSeg.magnitude - moveVector.magnitude;
                    extraDist = Mathf.Abs(extraDist);
                    //We both calculate the next segment and calculate our movement down this segment in this line. 
                    //We do it backwards so we don't have to invert it later
                    Vector2 dirWMag = (room.pathNodes[curSeg] - room.pathNodes[curSeg + 1]).normalized * extraDist;
                    //We move curSeg back up since we will be using the end of the segment, not the start to add the direction onto
                    curSeg++;
                    //In this line, we turn dirWMag into a Vector3 correctly and calculate the position of the start of the segments node as a Vector3
                    point = new Vector3(room.pathNodes[curSeg].x, position.y, room.pathNodes[curSeg].y) + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
                }
            }
            else
            {   //Else, calc a new point along this segment (using right & moveVec.mag) then recalculate moveVec
                Vector2 dirWMag = (-segVec.normalized) * moveVector.magnitude;
                point = pos + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
            }
        }
        else
        {   //We are moving right
            if (distIntoSeg.magnitude + moveVector.magnitude > segVec.magnitude)
            {   //If so, calc a new point further into that segment, then recalculate moveVec
                //The distance we need to move along the next segment
                curSeg++;
                //Make sure we are not out of range
                if (curSeg >= room.pathNodes.Length - 1)
                {   //We are out of range so literally just extend the current segment
                    Vector2 dirWMag = segVec.normalized * moveVector.magnitude;
                    point = pos + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
                }
                else
                {   //Calculate the distance we need to move down the next segment
                    float extraDist = (distIntoSeg.magnitude + moveVector.magnitude) - segVec.magnitude;
                    //We both calculate the next segment and calculate our movement down this segment in this line
                    Vector2 dirWMag = (room.pathNodes[curSeg + 1] - room.pathNodes[curSeg]).normalized * extraDist;
                    //In this line, we turn dirWMag into a Vector3 correctly and calculate the position of the start of the segments node as a Vector3
                    point = new Vector3(room.pathNodes[curSeg].x, position.y, room.pathNodes[curSeg].y) + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
                }
            }
            else
            {   //Else, calc a new point along this segment (using right & moveVec.mag) then recalculate moveVec
                Vector2 dirWMag = segVec.normalized * moveVector.magnitude;
                point = pos + new Vector3(dirWMag.x, moveVector.y, dirWMag.y);
            }
        }
        //Recalculate the movementVector
        moveVector = point - position;

        return moveVector;
    }
}
