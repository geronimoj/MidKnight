using UnityEngine;

public class RoomExit : MonoBehaviour
{
    //Rooms
    public GameObject currentRoom;
    public Room nextRoom;
    //Managers
    public GameManager GM;
    public EnemyManager EM;
    [SerializeField]
    private uint entranceIndex = 0;

    //Find the managers on start
    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        EM = FindObjectOfType<EnemyManager>();
    }

    //On trigger, moves the player to the entrance, spawns the new room and destroys the old one
    private void OnTriggerEnter(Collider other)
    {
        GM.room = nextRoom;
        other.GetComponent<CharacterController>().enabled = false;
        other.transform.position = nextRoom.entrances[entranceIndex];
        other.GetComponent<CharacterController>().enabled = true;
        Instantiate(nextRoom.roomPrefab);
        Destroy(currentRoom);
    }
}
