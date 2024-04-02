using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSpace : MonoBehaviour
{
    public bool isSpace = true;
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Floor")){
            isSpace = false;
        }
    }
}
