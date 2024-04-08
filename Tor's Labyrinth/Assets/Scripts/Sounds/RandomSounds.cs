using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioSource monsterSound;


    void Update(){
        if (Random.Range(1, 500000) == 1){
            wind.Play();
        }else if (Random.Range(1, 10000000) == 1){
            print("SECRET MESSAGE");
        }
    }
}
