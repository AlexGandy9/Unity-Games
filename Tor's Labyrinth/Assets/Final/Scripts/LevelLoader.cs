using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transistionTime = 1f;

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextLevel(){
        StartCoroutine(LoadLevel("Singleplayer"));
    }

    IEnumerator LoadLevel(string levelName){
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transistionTime);
        
        SceneManager.LoadScene(levelName);
    }
}
