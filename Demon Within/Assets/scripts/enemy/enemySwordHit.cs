using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySwordHit : MonoBehaviour
{

    public float SwordDamage;
    public float knockbackForce = 1500f;
    public Collider2D swordCollider;
    [SerializeField] public AudioSource swordHit;

    public GameObject enemy;

    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.CompareTag("Player")){
            swordHit.Play();
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = (Vector2)(parentPosition - enemy.transform.position).normalized;
        PlayerMovement.INSTANCE.OnHit(SwordDamage, direction * knockbackForce);
        }
    }
    // void OnCollisionExit2D(Collision2D col)
    // {

    //     Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

    //     Vector2 direction = (Vector2)(parentPosition - enemy.transform.position).normalized;
    //     PlayerMovement.INSTANCE.OnHit(SwordDamage, direction * knockbackForce);
    // }

    
}
