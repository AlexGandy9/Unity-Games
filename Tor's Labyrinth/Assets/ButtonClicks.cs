using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClicks : MonoBehaviour
{
    public Button killButton;
    public Button winButton;

    // Start is called before the first frame update
    void Start()
    {
        killButton.onClick.AddListener(() => SceneManager.LoadScene("Homepage"));
        winButton.onClick.AddListener(() => SceneManager.LoadScene("Homepage"));
        killButton.onClick.AddListener(() => KillPlayer.isKilled = false);
        winButton.onClick.AddListener(() => KillPlayer.isKilled = false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
