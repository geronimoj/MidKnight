using UnityEngine;

public class RoomExit : MonoBehaviour
{
    //Rooms
    public Room currentRoom;
    public Room nextRoom;
    //Managers
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private uint entranceIndex = 0;

    //Find the managers on start
    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    //On trigger, moves the player to the entrance of the next room, sets the next room active and destroys the old one
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        if (nextRoom == null || nextRoom.entrances.Length == 0 || entranceIndex >= nextRoom.entrances.Length)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = currentRoom.entrances[0];
            other.GetComponent<CharacterController>().enabled = true;
            Debug.LogWarning("Next room unassigned, has no entrances or the entranceIndex is invalid. Entering current room.");
            return;
        }
        GM.room = nextRoom;
        other.GetComponent<CharacterController>().enabled = false;
        other.transform.position = nextRoom.entrances[entranceIndex];
        other.GetComponent<CharacterController>().enabled = true;
        nextRoom.InstantiateRoom();
        Destroy(currentRoom.gameObject);
    }
}
