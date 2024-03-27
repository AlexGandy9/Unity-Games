using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    public Button btn1;
    public Button btn2;

    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(() => SceneManager.LoadScene("testScene2"));
        btn2.onClick.AddListener(() => Application.Quit());
    }
}
