using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExit : MonoBehaviour
{
    public GameObject currentRoom;
    public Room nextRoom;
    public GameManager GM;
    public EnemyManager EM;
    [SerializeField]
    private uint entranceIndex = 0;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        EM = FindObjectOfType<EnemyManager>();
    }

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
