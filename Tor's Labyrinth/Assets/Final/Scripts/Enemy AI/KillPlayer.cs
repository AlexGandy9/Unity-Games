using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillPlayer : MonoBehaviour
{
    public static bool isKilled;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (EnemyAI.animator.GetBool("isAttacking") && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.46f && EnemyAI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f){
                isKilled = true;
                EnemyAI.animator.SetBool("isKilled", true);
            }
        }
    }
}
