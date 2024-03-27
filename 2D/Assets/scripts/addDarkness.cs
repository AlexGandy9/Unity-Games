using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addDarkness : MonoBehaviour
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
                PlayerMovement.enemiesKilled++;
        if (PlayerMovement.enemiesKilled == 3)
        {
            PlayerMovement.darkness += 0.09f;
            PlayerMovement.enemiesKilled = 0;
        }
        else{
            PlayerMovement.darkness += 0.08f;
        }
            // PlayerPrefs.SetInt("darkness", PlayerPrefs.GetInt("darkness") + );
        }
        }
        
    }
}
