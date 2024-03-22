using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    [SerializeField] GameObject openingDoor;
    // Update is called once per frame
    void Update()
    {
        if (RoomTemplates.navMeshBuilt){
            if (openingDoor.transform.localRotation.y >= 0.2f){
                openingDoor.transform.Rotate(0, openingDoor.transform.localRotation.y - (0.0000001f * (openingDoor.transform.localRotation.y - 0.1f)), 0);
            }
        }
    }
}
