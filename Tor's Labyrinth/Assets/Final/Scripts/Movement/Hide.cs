using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public static bool hiding = false;
    private bool inTrigger = false;

    // Update is called once per frame
    void Update()
    {
        if (inTrigger){
            if (Input.GetKeyDown(KeyCode.LeftShift) && hiding == false){
                CharacterControllerMovement.SetPos(pos1);
                //Play Creaking sound
                hiding = true;
            }else if (Input.GetKeyDown(KeyCode.LeftShift) && hiding == true){
                CharacterControllerMovement.SetPos(pos2);
                hiding = false;
                //Play Creaking sound
            }
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player")){
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            inTrigger = false;
        }
    }
}
