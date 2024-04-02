using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    public static bool isAwake = false;

    void Update(){
        if (transform.rotation.z >= 0){
            transform.Rotate(-0.1f, 0, 0);
        }else {
            isAwake = true;
        }
    }
}
