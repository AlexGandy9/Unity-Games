using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillPlayerMP : MonoBehaviour
{
    public static bool isKilled;
    public AudioSource attackSound;

    void Awake(){
        isKilled = false;
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (EnemyController.animator.GetCurrentAnimatorStateInfo(0).IsName("attack1") && EnemyController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f && EnemyController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f){
                isKilled = true;
                EnemyController.animator.SetBool("isKilled", true);
            }
        }
    }
}
