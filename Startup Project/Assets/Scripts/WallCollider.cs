using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    //private bool alreadyDead = false;

    /*void OnCollisionEnter(Collision collision){
        if (!alreadyDead){
            WallCollider collider = collision.gameObject.GetComponent<WallCollider>();
            if (collider != null){
                collider.alreadyDead = true;
                Destroy(collision.gameObject);
            }
        }
    }*/
    private void OnCollisionEnter(Collision other)
    {  

        if (other.gameObject.CompareTag("Wall"))
            { 
                if (gameObject.GetInstanceID() > other.gameObject.GetInstanceID())
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
    }
}
