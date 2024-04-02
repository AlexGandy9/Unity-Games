using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    [SerializeField] GameObject openingDoor;
    [SerializeField] AudioSource creak;
    // Update is called once per frame
    void Start(){
        print(openingDoor.transform.localRotation);
        openingDoor.transform.localRotation = new Quaternion(0, 0.70711f, 0, -0.70711f);
    }
    void FixedUpdate()
    {
        if (RoomTemplates.navMeshBuilt){
            if (openingDoor.transform.localRotation.y >= 0.2f){
                if (!creak.isPlaying){
                    creak.Play();
                }
                openingDoor.transform.Rotate(0, openingDoor.transform.localRotation.y - (0.35f * (openingDoor.transform.localRotation.y)), 0);
            }
        }
    }
}
