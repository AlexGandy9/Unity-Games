using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {  
        if (other.CompareTag("Wall") || other.gameObject.name == "Floor")
            { 
                if (gameObject.GetInstanceID() < other.gameObject.GetInstanceID())
                {
                    Destroy(gameObject);
                }else {
                    Destroy(other.gameObject);
                }
            }
    }
}
