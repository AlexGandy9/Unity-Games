using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public GameObject transition;

    public static SceneTransition INSTANCE;

    private Animator transition2;

    public string sceneToLoad;
    public Vector2 playerPos;

    public float transitionTime = 0.5f;
    public VectorValue playerStoredPos;

    public GameObject player;

    private Rigidbody2D rb;

    public bool choiceTransition = false;

    public string decider;

    public bool choiceValue;

    public float cValue;

    public string sceneToLoad2;

    private string stl2;



    // Start is called before the first frame update

    void Start()
    {

        INSTANCE = this;

        rb = player.GetComponent<Rigidbody2D>();
        transition2 = transition.GetComponent<Animator>();
        Debug.Log("this " + decider + PlayerPrefs.GetInt(decider));

    }

    
    public void OnTriggerEnter2D(Collider2D other)
    {

        if(choiceTransition){
            if(choiceValue){
                if(cValue > PlayerPrefs.GetInt(decider)){
                    stl2 = sceneToLoad2;
                }
                else{
                    stl2 = sceneToLoad;
                }
            }
            else{
                if(PlayerPrefs.GetInt(decider) == 1){
                stl2 = sceneToLoad2;
                
            }
            else{
                stl2 = sceneToLoad;
            }
            }
            
        }
        else{
            stl2 = sceneToLoad;
        }



        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerStoredPos.initialValue = playerPos;

            StartCoroutine(LoadNextScene(stl2));
        }
        
    }

    public void loadScene(string sceneToLoad){
        StartCoroutine(LoadNextScene(sceneToLoad));

    }

    IEnumerator LoadNextScene(string stl)
    {
        
        Debug.Log("wwd" + PlayerPrefs.GetInt("workWithDemons"));
        PlayerAttack.INSTANCE.doorCheck = true;
        PlayerMovement.INSTANCE.canMove = false;
        transition2.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(stl);

    }
}
