using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillPlayer : MonoBehaviour
{
    void Update(){
    }
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            print(EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (EnemyAI.animator.GetBool("isAttacking") && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.5f && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.63f){
                Destroy(other.gameObject);
                //SceneManager.LoadScene("Singleplayer");
                EnemyAI.animator.SetBool("isKilled", true);
            }
        }
    }
}
