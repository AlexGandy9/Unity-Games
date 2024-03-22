using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySP : MonoBehaviour
{
    public static bool destroy = true;

    void Start(){
        destroy = false;
    }

    public static bool GetDestroy(){
        return destroy;
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("SpawnPoint")){
            Destroy(other.gameObject);
        }
    }
}
