using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
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

    public float waitTime;
    private bool spawnedExit;
    private bool spawnedExit1;
    private bool spawnedExit2;
    private bool torSpawned;
    private int room1;
    private int room2;
    public GameObject exit;
    public NavMeshSurface surface;
    public GameObject Tor;
    public static bool navMeshBuilt;

    void Awake(){
        navMeshBuilt = false;
    }

    void Update(){
        if (waitTime <= 0 && spawnedExit2 == false){
            foreach (GameObject room in rooms){
                if (room.name.Length == 8){
                    endRooms.Add(room);
                }
            }

            if (endRooms.Count < 2){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            //Spawns the first exit in relation to where the spawn for the player is
            for (int i = endRooms.Count - 1; i >= 0; i--){
                float dist1 = Vector3.Distance(endRooms[i].transform.position, floor.transform.position);
                if (dist1 > 60f && spawnedExit1 == false){
                    //Just in case
                    spawnedExit1 = true;
                    room1 = i;
                }
            }

            
            //Spawns the second room in relation to the first
            for (int i = endRooms.Count - 1; i >= 0; i--){
                float dist2 = Vector3.Distance(endRooms[room1].transform.position, endRooms[i].transform.position); 
                if (dist2 > 40f && spawnedExit2 == false && dist2 < 80f && i != room1){
                    //Just in case
                    room2 = i;
                    spawnedExit2 = true;
                }
            }

            if (spawnedExit2 == false){
                for (int i = endRooms.Count - 1; i >= 0; i--){
                    float dist2 = Vector3.Distance(endRooms[room1].transform.position, endRooms[i].transform.position);
                    if (spawnedExit2 == false && dist2 < 80f && i != room1){
                        //Just in case 
                        room2 = i;
                        spawnedExit2 = true;
                    }
                }
            }
        }else {
            waitTime -= Time.deltaTime;
        }

        if (spawnedExit2 == true && spawnedExit1 == true && navMeshBuilt == false){
            surface.BuildNavMesh();
            navMeshBuilt = true;
        }

        if (spawnedExit2 == true && !torSpawned && endRooms[room1] != null && navMeshBuilt){
            foreach (GameObject room in rooms){
                float dist3_1 = Vector3.Distance(endRooms[room1].transform.position, room.transform.position);
                float dist3_2 = Vector3.Distance(endRooms[room2].transform.position, room.transform.position);
                if (dist3_1 < 60f && dist3_2 < 60f){
                    Instantiate(Tor, room.transform.position, Quaternion.identity);
                    torSpawned = true;
                    break;
                }
            }
        }

        try{
            if (waitTime <= 0 && endRooms[room1] != null){
            Instantiate(exit, endRooms[room2].transform.position, Quaternion.identity);
            endRooms[room2].active = false;
            Instantiate(exit, endRooms[room1].transform.position, Quaternion.identity);
            endRooms[room1].active = false;
            }
        }catch {
            return;
        }
    }
}
