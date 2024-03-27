using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector cutscene;

    public bool cutsceneDoneAlready = false;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(!cutsceneDoneAlready){
                cutsceneDoneAlready = true;
                cutscene.Play();
            }
        }
    }
}
