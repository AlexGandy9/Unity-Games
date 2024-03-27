using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class swordHitBox : MonoBehaviour
{

    public float SwordDamage = 1f;
    public float knockbackForce;
    public Collider2D swordCollider;
    public GameObject player;

    [SerializeField] public AudioSource swordHit;

    void OnTriggerEnter2D(Collider2D col)
    {


        IDamagable damagableObject = col.GetComponent<Collider2D>().GetComponent<IDamagable>();

        if (damagableObject != null)
        {
            swordHit.Play();
            Vector3 parentPosition = transform.parent.position;

            Vector2 direction = (col.GetComponent<Collider2D>().gameObject.transform.position - player.transform.position).normalized;
           
            Vector2 knockback = direction * knockbackForce;

            damagableObject.OnHit(PlayerAttack.damage, knockback);

        }
    }
}
