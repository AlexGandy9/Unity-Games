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

    public static int roomsSpawned = 0;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.2f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (spawned == false){
            if (this.gameObject.name.Contains("SpawnStart")){
                Instantiate(templates.startRoom, transform.position, templates.startRoom.transform.rotation);
            }
            if (roomsSpawned <= 50){
                rand = Random.Range(1, 6);
            }else {
                rand = Random.Range(0, 6);
            }
            if (roomsSpawned >= 100){
                if(openingDirection == 2){
                Instantiate(templates.bottomRooms[0], transform.position, templates.bottomRooms[0].transform.rotation);
                }else if (openingDirection == 1){
                    Instantiate(templates.topRooms[0], transform.position, templates.topRooms[0].transform.rotation);
                }else if (openingDirection == 4){
                    Instantiate(templates.leftRooms[0], transform.position, templates.leftRooms[0].transform.rotation);
                }else if (openingDirection == 3){
                    Instantiate(templates.rightRooms[0], transform.position, templates.rightRooms[0].transform.rotation);
                }
            }else if(openingDirection == 2){
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }else if (openingDirection == 1){
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }else if (openingDirection == 4){
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }else if (openingDirection == 3){
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
            roomsSpawned += 1;
            //Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other){
        if (other.CompareTag("Floor")){
            Destroy(gameObject);
        }
        if (other.CompareTag("SpawnPoint")){
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false && DestroySP.destroy == false){
                Instantiate(templates.rightRooms[7], transform.position, templates.rightRooms[7].transform.rotation);
            }
            spawned = true;
        }else{
            
        }
    }
}
