using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioSource monsterSound;


    void FixedUpdate(){
        if (Random.Range(1, 500000) == 1){
            wind.Play();
        }
    }
}
