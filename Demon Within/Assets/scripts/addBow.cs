using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addBow : MonoBehaviour
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

        if(set){
                done = true;
            PlayerPrefs.SetInt("bow", 1);
        }
        }
        
    }

