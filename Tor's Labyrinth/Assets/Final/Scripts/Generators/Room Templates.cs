
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    //Rooms 
    public GameObject floor;
    public GameObject startRoom;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] landmark1;
    public GameObject[] landmark2;
    public GameObject shutRoom;
    public List<GameObject> rooms;
    public List<GameObject> endRooms;
    private int room1;
    private int room2;
    public GameObject exit;
    //Room booleans
    private bool spawnedExit;
    private bool spawnedExit1;
    private bool spawnedExit2;
    private bool setExits = false;
    private float maxDist = 120f;
    private float maxDist2 = 50f;

    //Tor spawning
    public GameObject Tor;
    [SerializeField] private CharacterController controller;
    public static bool torSpawned;
    private string sceneName;


    //NavMesh 
    public NavMeshSurface surface;
    public static bool navMeshBuilt;
    public float waitTime;

    void Awake(){
        navMeshBuilt = false;
        torSpawned = false;
        spawnedExit = false;
        spawnedExit1 = false;
        spawnedExit2 = false;

        RoomSpawner.l1 = false;
        RoomSpawner.l2T = 0;
        RoomSpawner.l2Pos = Vector3.zero;

        sceneName = SceneManager.GetActiveScene().name;
    }

    void Update(){
        if (waitTime <= 0 && !spawnedExit2){
            foreach (GameObject room in rooms){
                if (room.name.Length == 8){
                    endRooms.Add(room);
                }
            }
            print(rooms.Count);
            if (rooms.Count < 70){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            //Spawns the first exit in relation to where the spawn for the player is
            while (!spawnedExit1){
                for (int i = endRooms.Count - 1; i >= 0; i--){
                    float dist1 = Vector3.Distance(endRooms[i].transform.position, floor.transform.position);
                    if (dist1 >= maxDist && spawnedExit1 == false){
                        //Just in case
                        spawnedExit1 = true;
                        room1 = i;
                    }
                }
                maxDist -= 10f;
            }

            
            //Spawns the second room in relation to the first
            while (!spawnedExit2){
                for (int i = endRooms.Count - 1; i >= 0; i--){
                    float dist2 = Vector3.Distance(endRooms[room1].transform.position, endRooms[i].transform.position); 
                    if (dist2 <= 80f && spawnedExit2 == false && dist2 >= maxDist2 && i != room1){
                        room2 = i;
                        spawnedExit2 = true;
                    }
                }
                maxDist2 -= 10f;
            }

        }else {
            waitTime -= Time.deltaTime;
        }

        if (spawnedExit2 && spawnedExit1 && !navMeshBuilt){
            if (sceneName != "Multiplayer"){
                surface.BuildNavMesh();
            }
            navMeshBuilt = true;
        }

        /*if (spawnedExit2 && !torSpawned && endRooms[room1] != null && navMeshBuilt){
            foreach (GameObject room in rooms){
                float dist3_1 = Vector3.Distance(endRooms[room1].transform.position, room.transform.position);
                float dist3_2 = Vector3.Distance(endRooms[room2].transform.position, room.transform.position);
                if (dist3_1 < 50f && dist3_2 < 50f && dist3_1 > 10f && dist3_2 > 10f){
                    torSpawned = true;
                    if (sceneName.Equals("SingleplayerEasy") || sceneName.Equals("SingleplayerHard") || sceneName.Equals("SingleplayerMoving")){
                        Instantiate(Tor, room.transform.position, Quaternion.identity);
                    } else if (sceneName.Equals("Multiplayer")){
                        controller.enabled = false;
                        Tor.transform.position = room.transform.position;
                        controller.enabled = true;
                    }
                    break;
                }
            }
        }*/

        float maxDist3 = 50f;
        if (spawnedExit2 && !torSpawned && endRooms[room1] != null && navMeshBuilt){
            while (!torSpawned){
                foreach (GameObject room in rooms){
                    float dist3_1 = Vector3.Distance(endRooms[room1].transform.position, room.transform.position);
                    float dist3_2 = Vector3.Distance(endRooms[room2].transform.position, room.transform.position);
                    if (dist3_1 < maxDist3 && dist3_2 < maxDist3 && dist3_1 > 10f && dist3_2 > 10f){
                        torSpawned = true;
                        if (sceneName.Equals("SingleplayerEasy") || sceneName.Equals("SingleplayerHard") || sceneName.Equals("SingleplayerMoving")){
                            Instantiate(Tor, room.transform.position, Quaternion.identity);
                        } else if (sceneName.Equals("Multiplayer")){
                            controller.enabled = false;
                            Tor.transform.position = room.transform.position;
                            controller.enabled = true;
                        }
                        break;
                    }
                }
                maxDist3 += 10f;
            }
        }

        if (waitTime <= 0 && !setExits){
            Instantiate(exit, endRooms[room2].transform.position, Quaternion.identity);
            endRooms[room2].transform.Find("Floor").gameObject.SetActive(false);
            Instantiate(exit, endRooms[room1].transform.position, Quaternion.identity);
            endRooms[room1].transform.Find("Floor").gameObject.SetActive(false);
            setExits = true;
        }
    }
}
