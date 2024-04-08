using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    public void OpenSinglePlayer(){
        StartCoroutine(LoadLevel("Singleplayer"));
    }

    public void OpenMultiPlayer(){
        StartCoroutine(LoadLevel("Multiplayer"));
    }

    public void OpenOptions(){
        SceneManager.LoadScene("Options");
    }

    public void ExitGame(){
        Application.Quit();
    }
    IEnumerator LoadLevel(string levelName){
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(0);
        
        SceneManager.LoadScene(levelName);
    }
}
