using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RoomExit_Test
    {
        [UnityTest]
        public IEnumerator RoomExit_ChangeRoom()
        {
            ScreenFader_Test.CreateScreenFader();
            //Spawn a player
            PlayerController p = Player.CreatePlayer();
            p.transform.position = new Vector3(0, 10, 0);
            //Get the gamemanager
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            //Spawn an exit
            GameObject exit = CreateRoomExit();
            //Spawn a room and assign it to the game manager
            gm.room = CreateRoom();
            //Spawn another room and assign it to exit
            Room target = CreateRoom();
            exit.GetComponent<RoomExit>().nextRoom = target;
            target.roomID = "TargetID";

            yield return null;
            //Once the collision occurs, check that the screenfader is fading in.
            //Drop the player onto the exit to cause a collision
            p.Move(Vector3.down * 0);
            yield return new WaitForSeconds(5);
            //Check that the rooms have been swapped over
            Debug.Assert(gm.room.roomID == "TargetID");
            //Check that the screen is fading out.
        }

        public static GameObject CreateRoomExit()
        {
            GameObject g = GameObject.Instantiate(new GameObject());

            g.AddComponent<RoomExit>();
            g.GetComponent<BoxCollider>().isTrigger = true;
            g.name = "Room";

            return g;
        }

        public static Room CreateRoom()
        {
            GameObject g = GameObject.Instantiate(new GameObject());

            Room r =  g.AddComponent<Room>();
            r.roomID = "Valid";

            r.entrances = new Vector3[1];
            r.pathNodes = new Vector2[0];

            return r;
        }
    }
}
