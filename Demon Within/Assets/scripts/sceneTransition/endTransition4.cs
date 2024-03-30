using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class endTransition4 : MonoBehaviour
{
    public GameObject transition;

    public static endTransition4 INSTANCE;

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

    public int cValue;

    public string sceneToLoad2;

    private string stl2;
    // Start is called before the first frame update
    void Start()
    {

        INSTANCE = this;

        rb = player.GetComponent<Rigidbody2D>();
        transition2 = transition.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

       
            stl2 = "titlePage";



        if (other.CompareTag("Player") && !other.isTrigger)
        {
            

            StartCoroutine(LoadNextScene(stl2));
        }
        
    }

    public void loadScene(string sceneToLoad){
        StartCoroutine(LoadNextScene(sceneToLoad));

    }

    IEnumerator LoadNextScene(string stl)
    {
        Debug.Log("here");
        PlayerAttack.INSTANCE.doorCheck = true;
        PlayerMovement.INSTANCE.canMove = false;
        transition2.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(stl);

    }
}
