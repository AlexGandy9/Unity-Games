using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLandmark : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Wall") || other.CompareTag("SpawnPoint") || other.CompareTag("ObstaclesGenerator")){
            Destroy(other.gameObject);
        }
    }
}
