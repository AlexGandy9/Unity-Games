using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrap : MonoBehaviour
{
    public static bool isSlowed = false;
    private float waitTime = 3f;

    void Update(){
        if (isSlowed){
            if (waitTime <= 0){
                isSlowed = false;
            }else if (waitTime > 0){
                waitTime -= Time.deltaTime;
            }
        }
        if (isSlowed){
            print("SLOWED DOWN");
        }else {
            print("SPED UP");
        }

        if (waitTime <= 0 && !isSlowed){
            waitTime = 3f;
        }
    }
   /* private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            print("TRAPPED");
            //Play Trap Sound
            waitTime = 3f;
            isSlowed = true;
        }
    }*/

    /*private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player")){
            waitTime = 3f;
            isSlowed = true;
        }
    }*/

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            waitTime = 3f;
            isSlowed = true;
        }
    }
}
