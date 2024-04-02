using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillPlayer : MonoBehaviour
{
    public static bool isKilled;

    void Awake(){
        isKilled = false;
    }
    void Update(){
    }
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (EnemyAI.animator.GetBool("isAttacking") && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.46f && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f){
                isKilled = true;
                //SceneManager.LoadScene("Singleplayer");
                EnemyAI.animator.SetBool("isKilled", true);
            }
        }
    }
}
