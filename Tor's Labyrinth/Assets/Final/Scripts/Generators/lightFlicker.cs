using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    public Light lightSource;
    // Update is called once per frame
    void Start(){
        RenderSettings.ambientLight = Color.black;
    }
    void Update()
    {
        int rand = Random.Range(0, 500);
        if (rand < 2 && lightSource.enabled){
            //play some flicker sound? (should be quiet)
            lightSource.enabled = false;
        }else if (rand < 40 && !lightSource.enabled){
            lightSource.enabled = true;
        }
    }
}
