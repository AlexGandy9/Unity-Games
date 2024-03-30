using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetVars : MonoBehaviour
{

    private bool done = false;
    public bool set = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(!done){

            if(set){
                done = true;
            PlayerPrefs.SetInt("shovel", 0);
            PlayerPrefs.SetInt("mShield", 0);
            PlayerPrefs.SetInt("ladder", 0);
            PlayerPrefs.SetInt("flowers", 0);
            PlayerPrefs.SetInt("dug", 0);
            PlayerPrefs.SetInt("darkness", 0);
            PlayerPrefs.SetInt("bow", 0);
            PlayerPrefs.SetInt("battleP", 0);
            PlayerPrefs.SetInt("cantMake", 0);
            PlayerPrefs.SetInt("pshield", 0);
            PlayerPrefs.SetInt("bought", 0);
            PlayerPrefs.SetInt("workWithDemons", 0);
            PlayerPrefs.SetInt("getBow", 0);
            PlayerPrefs.SetInt("talked", 0);
            PlayerPrefs.SetInt("bought", 0);
            PlayerMovement.health = PlayerMovement.healthStart;
            PlayerAttack.damage = 0;
            PlayerMovement.enemiesKilled = 0;
            PlayerMovement.darkness = 0;

        }
        }
        
    }
}
