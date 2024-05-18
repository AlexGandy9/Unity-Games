using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        if (other.CompareTag("Enemy")){
            other.transform.position = other.transform.position;
            print(other.transform.position);
        }
    }
}
