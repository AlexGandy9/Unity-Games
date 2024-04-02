using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpPlayer : MonoBehaviour
{

    [SerializeField] private Canvas playerLight;
    // Update is called once per frame
    void Update()
    {
        if (HitTrap2.isStuck){
            playerLight.enabled = true;
        }else if (!HitTrap2.isStuck){
            playerLight.enabled = false;
        }
        
        if (HitTrap.isSlowed){
            playerLight.enabled = true;
        }else if (!HitTrap.isSlowed){
            playerLight.enabled = false;
        }
    }
}
