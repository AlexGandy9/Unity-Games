using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OpenSinglePlayer(){
        SceneManager.LoadScene("Singleplayer");
    }

    public void OpenMultiPlayer(){
        SceneManager.LoadScene("Multiplayer");
    }

    public void OpenOptions(){
        SceneManager.LoadScene("Options");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
