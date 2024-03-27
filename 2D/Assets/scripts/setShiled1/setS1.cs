using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setS1 : MonoBehaviour
{

    private bool done = false;
    public bool set = false;
    public bool set2 = false;
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
                PlayerPrefs.SetInt("cantMake", 1);
            }
            if(set2){
                done = true;
                PlayerPrefs.SetInt("cantMake", 0);
            }
        }
        

        
    }
}
