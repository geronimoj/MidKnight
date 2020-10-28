using UnityEngine;

public class RoomExit : MonoBehaviour
{
    //Rooms
    public Room currentRoom;
    public Room nextRoom;
    //Managers
    public GameManager GM;
    public EntitiesManager EM;
    [SerializeField]
    private uint entranceIndex = 0;

    //Find the managers on start
    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        EM = FindObjectOfType<EntitiesManager>();
    }

    //On trigger, moves the player to the entrance, sets the new room active and deactivates the old one
    private void OnTriggerEnter(Collider other)
    {
        GM.room = nextRoom;
        other.GetComponent<CharacterController>().enabled = false;
        other.transform.position = nextRoom.entrances[entranceIndex];
        other.GetComponent<CharacterController>().enabled = true;
        nextRoom.InstantiateRoom();
        Destroy(currentRoom.gameObject);
        //currentRoom.gameObject.SetActive(false);
    }
}
