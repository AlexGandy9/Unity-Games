using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    private GameObject music;

    public void Awake(){
        music = GameObject.Find("Audio");
        DontDestroyOnLoad(music);
    }

    public void OpenSinglePlayerEasy(){
        Destroy(music);
        StartCoroutine(LoadLevel("SingleplayerEasy"));
    }

    public void OpenSinglePlayerHard(){
        Destroy(music);
        StartCoroutine(LoadLevel("SingleplayerHard"));
    }

    public void OpenSinglePlayerMoving(){
        Destroy(music);
        StartCoroutine(LoadLevel("SingleplayerMoving"));
    }

    public void OpenSinglePlayerScreen(){
        StartCoroutine(LoadLevel("SingleHomepage"));

    }

    public void OpenMultiPlayerScreen(){
        StartCoroutine(LoadLevel("MultiHomepage"));
    }

    public void OpenHomepage(){
        StartCoroutine(LoadLevel("Homepage"));
    }

    public void OpenMultiPlayer(){
        Destroy(music);
        StartCoroutine(LoadLevel("Multiplayer"));
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
