using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    private GameObject startRoom;
    private GameObject[] landmark1;
    private static bool l1 = false;
    private static int l2T = 0;
    private static Vector3 l2Pos = new Vector3(0, 0, 0);
    private GameObject[] landmark2;
    private GameObject[] bottomRooms;
    private GameObject[] topRooms;
    private GameObject[] leftRooms;
    private GameObject[] rightRooms;

    [SerializeField] private IsSpace isSpace;

    public static int roomsSpawned;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        startRoom = templates.startRoom;
        bottomRooms = templates.bottomRooms;
        topRooms = templates.topRooms;
        leftRooms = templates.leftRooms;
        rightRooms = templates.rightRooms;
        landmark1 = templates.landmark1;
        landmark2 = templates.landmark2;

        Invoke("Spawn", 0.3f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (!spawned){
            if (this.gameObject.name.Contains("SpawnStart")){
                Instantiate(startRoom, transform.position, startRoom.transform.rotation);
                roomsSpawned = 0;
            }
            if ((isSpace.isSpace && roomsSpawned >= 15 && !l1)){
                Instantiate(landmark1[openingDirection-1], transform.position, landmark1[openingDirection-1].transform.rotation);
                l1 = true;
            } else if (isSpace.isSpace && l2T < 2 && Vector3.Distance(transform.position, l2Pos) > 100f){
                Instantiate(landmark2[openingDirection-1], transform.position, landmark2[openingDirection-1].transform.rotation);
                l2Pos = transform.position;
                l2T++;
            } else if (roomsSpawned <= 50){
                rand = Random.Range(1, 6);
                SpawnRoom(rand);        
            }else {
                rand = Random.Range(0, 6);
                SpawnRoom(rand);
            }

            spawned = true;
            roomsSpawned += 1;
        }
    }

    private void SpawnRoom(int rand){
        if (roomsSpawned >= 100){
            if(openingDirection == 2){
            Instantiate(bottomRooms[0], transform.position, bottomRooms[0].transform.rotation);
            }else if (openingDirection == 1){
                Instantiate(topRooms[0], transform.position, topRooms[0].transform.rotation);
            }else if (openingDirection == 4){
                Instantiate(leftRooms[0], transform.position, leftRooms[0].transform.rotation);
            }else if (openingDirection == 3){
                Instantiate(rightRooms[0], transform.position, rightRooms[0].transform.rotation);
            }
        }else if(openingDirection == 2){
            Instantiate(bottomRooms[rand], transform.position, bottomRooms[rand].transform.rotation);
        }else if (openingDirection == 1){
            Instantiate(topRooms[rand], transform.position, topRooms[rand].transform.rotation);
        }else if (openingDirection == 4){
            Instantiate(leftRooms[rand], transform.position, leftRooms[rand].transform.rotation);
        }else if (openingDirection == 3){
            Instantiate(rightRooms[rand], transform.position, rightRooms[rand].transform.rotation);
        }
    }

    void OnTriggerStay(Collider other){
        if (other.CompareTag("Floor")){
            Destroy(gameObject);
        }
        if (other.CompareTag("SpawnPoint")){
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false && DestroySP.destroy == false){
                Instantiate(templates.shutRoom, transform.position, templates.shutRoom.transform.rotation);
            }
            spawned = true;
        }else{
            
        }
    }
}
