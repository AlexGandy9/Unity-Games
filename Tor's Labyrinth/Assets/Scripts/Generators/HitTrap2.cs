using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrap2 : MonoBehaviour
{
    public static bool isStuck = false;
    public HealthBar spaces;
    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        if (isStuck){

            if (spaces.spaces <= 0){
                isStuck = false;
                canvas.gameObject.active = false;
            }else if (spaces.spaces > 0){
                if (Input.GetKeyDown(KeyCode.Space)){
                    spaces.RemoveSpace();
                    //update ui in relation to keyboardHitsLeft
                }
            }
        }

        if (spaces.spaces <= 0 && !isStuck){
            spaces.spaces = 10;
            spaces.UpdateSpace();
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            isStuck = true;
            canvas.gameObject.active = true;
        }
    }
}
