using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    public Light light;
    // Update is called once per frame
    void Start(){
        RenderSettings.ambientLight = Color.black;
    }
    void Update()
    {
        int rand = Random.Range(0, 500);
        if (rand < 2 && light.enabled){
            //play some flicker sound? (should be quiet)
            light.enabled = false;
        }else if (rand < 40 && !light.enabled){
            light.enabled = true;
        }
    }
}
