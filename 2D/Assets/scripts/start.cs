using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{

    public string stl;

    public GameObject transition;

    private Animator transition2;

    [SerializeField] public AudioSource menuSound;


    // Start is called before the first frame update
    void Start()
    {
        transition2 = transition.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
                {
                    menuSound.Play();

                    StartCoroutine(LoadNextScene(stl));
                }
                
        
    }
    IEnumerator LoadNextScene(string stl2)
    {
        transition2.SetTrigger("Start");

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(stl2);

    }
}
