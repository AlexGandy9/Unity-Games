using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        print("Player Finished");
        if (other.CompareTag("Player")){
            Cursor.visible = true;
            CharacterControllerMovement.winScreen.gameObject.SetActive(true);
        }
    }
}
