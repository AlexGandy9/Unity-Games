using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerMove : MonoBehaviour
{
    public static bool isKilled;
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (EnemyAIMoving.animator.GetBool("isAttacking") && EnemyAIMoving.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.46f && EnemyAIMoving.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f){
                isKilled = true;
                EnemyAIMoving.animator.SetBool("isKilled", true);
            }
        }
    }
}
