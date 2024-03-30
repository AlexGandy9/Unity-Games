using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

public class dialogueHolder : MonoBehaviour
{

    public GameObject dBox;
    public TextMeshProUGUI dText;
    public GameObject cBox1;
    public TextMeshProUGUI c1Text;
    public GameObject cBox2;
    public TextMeshProUGUI c2Text;

    public bool isOpening; 

    public Image arrow1;
    public Image arrow2;

    private bool choice1;

    private string[] currentDialogue;

    public string npcName;
    public GameObject[] csystem;

    // public bool useCSystem2;
    public bool useCSystemDecider;

    public string cSystemDecider;
    public GameObject[] csystem2;
    private string[] dialogue;
    private string[] choices;

    private int currentChoice;
    private bool inRange;
    private int textP;
    public bool dialogueActive;

    private bool decisionTime;

    public bool cutsceneTrigger = false;
    private bool cutsceneStarted = false;

    public bool afterConversationAnimation;

    public bool cutsceneChoice;

    public bool cutsceneWhich;
    public GameObject cutscene;
    public GameObject cutscene2;
    private PlayableDirector cutscene1;

    private int decider;

    public string cutsceneEndDecider;

    public bool hasChoice = false;

    public string choiceName;

    public bool demonChoice = false;





    private int conversation;

    // Start is called before the first frame update
    void Start()
    {
        currentChoice = 0;

        decisionTime = false;
        // dMan = FindObjectOfType<DialogueManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if(cutsceneTrigger && !cutsceneStarted){
            PlayerMovement.INSTANCE.animator.enabled = false;
            cutsceneStarted = true;
            PlayerAttack.INSTANCE.talking = true;
            inRange = true;
            PlayerMovement.INSTANCE.canMove = false;
            dBox.SetActive(true);
            dialogueActive = true;
            ChoiceSystem cs2;
            currentChoice = 0;
            // if(useCSystem2){
            //     cs2 = (ChoiceSystem)csystem2[currentChoice].GetComponent("ChoiceSystem");
            // }
            // else{
                cs2 = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
            // }
            if(useCSystemDecider){
                    int csd = PlayerPrefs.GetInt(cSystemDecider);
                    if(csd == 1){
                        cs2 = (ChoiceSystem)csystem2[currentChoice].GetComponent("ChoiceSystem");
                    }
                    else{
                        cs2 = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
                    }
                }
            string[] dialogue2 = cs2.getText();
            string[] choices2 = cs2.getChoices();
            StartCoroutine(TypeSentence(dialogue2[0]));
            textP = 1;

        }
        if ((inRange && Input.GetKeyDown(KeyCode.Space)) || (cutsceneTrigger && Input.GetKeyDown(KeyCode.Space)))
        {
            StopAllCoroutines();
            PlayerMovement.INSTANCE.canMove = false;
            if (decisionTime)
            {
                if (choice1)
                {
                    currentChoice++;
                    textP = 0;
                    cBox1.SetActive(false);
                    cBox2.SetActive(false);
                    decisionTime = false;
                    if(hasChoice){
                        PlayerPrefs.SetInt(choiceName, 0);
                        if(demonChoice){
                            PlayerPrefs.SetInt(choiceName, 1);
                        }
                    }
                    if (isOpening)
                    {
                        PlayerMovement.darkness += 0.25f;
                    }
                }
                else
                {
                    currentChoice = currentChoice + 2;
                    textP = 0;
                    cBox1.SetActive(false);
                    cBox2.SetActive(false);
                    decisionTime = false;
                    if(hasChoice){
                        PlayerPrefs.SetInt(choiceName, 1);
                        if(demonChoice){
                            PlayerPrefs.SetInt(choiceName, 0);

                        }
                        
                    }
                }
                Debug.Log(choiceName + PlayerPrefs.GetInt(choiceName));
            }
            ChoiceSystem cs;
            if(useCSystemDecider){
                Debug.Log("current" + currentChoice);
                if(PlayerPrefs.GetInt(cSystemDecider) == 1){
                 cs = (ChoiceSystem)csystem2[currentChoice].GetComponent("ChoiceSystem");
                }
        
            else{
             cs = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
            }
            }
            else{
             cs = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
            }
            dialogue = cs.getText();
            choices = cs.getChoices();

            if (dialogueActive)
            {
                if (textP == dialogue.Length)
                {
                    if (choices.Length != 2)
                    {
                        dBox.SetActive(false);
                        dialogueActive = false;
                        PlayerMovement.INSTANCE.canMove = true;
                        if(cutsceneStarted){
                            inRange = false;
                            cutsceneTrigger = false;
                            cutsceneStarted = false;
                            PlayerAttack.INSTANCE.talking = false;
                             
                        }
                        PlayerMovement.INSTANCE.animator.enabled = true;
                        if(afterConversationAnimation){
                            afterConversationAnimation = false;
                            if(cutsceneChoice){
                            int decider = PlayerPrefs.GetInt(cutsceneEndDecider);
                            if(decider == 0){
                                cutscene1 = cutscene.GetComponent<PlayableDirector>();
                            }
                            else{
                                cutscene1 = cutscene2.GetComponent<PlayableDirector>();
                            }
                            }
                            else{
                                cutscene1 = cutscene.GetComponent<PlayableDirector>();
                            }
                            cutscene1.Play(); 
                        }
                    }
                    else
                    {
                        PlayerMovement.INSTANCE.canMove = false;
                        cBox1.SetActive(true);
                        c1Text.text = choices[0];
                        cBox2.SetActive(true);
                        c2Text.text = choices[1];
                        decisionTime = true;
                        arrow1.enabled = true;
                        arrow2.enabled = false;
                        choice1 = true;
                    }
                }
                else
                {
                  
                    StartCoroutine(TypeSentence(dialogue[textP]));
                    textP++;
                }
            }
            else
            {
            
                PlayerMovement.INSTANCE.canMove = false;
                dBox.SetActive(true);
                dialogueActive = true;
                
                currentChoice = 0;
                ChoiceSystem cs2;
                if(useCSystemDecider){
                    int csd = PlayerPrefs.GetInt(cSystemDecider);
                    if(csd == 1){
                        cs2 = (ChoiceSystem)csystem2[currentChoice].GetComponent("ChoiceSystem");
                    }
                    else{
                        cs2 = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
                    }
                }
                else{
                     cs2 = (ChoiceSystem)csystem[currentChoice].GetComponent("ChoiceSystem");
                }
                string[] dialogue2 = cs2.getText();
                string[] choices2 = cs2.getChoices();
                if (cs2.getUsePref() ==true){
                    for(int i = 0; i < dialogue2.Length; i++){
                        dialogue2[i].Replace("REPLACE", PlayerPrefs.GetString(cs2.getPref()).ToString());
                    }
                }
                StartCoroutine(TypeSentence(dialogue2[0]));
                textP = 1;

            }
        }
        if (decisionTime && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            arrow1.enabled = true;
            choice1 = true;
            arrow2.enabled = false;
        }
        if (decisionTime && Input.GetKeyDown(KeyCode.RightArrow))
        {
            arrow1.enabled = false;
            choice1 = false;
            arrow2.enabled = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dText.text = npcName + ": ";
        foreach (char letter in sentence.ToCharArray())
        {
            dText.text += letter;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerAttack.INSTANCE.talking = true;
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = false;
            if (dialogueActive)
            {
                dBox.SetActive(false);
                dialogueActive = false;
            }
            PlayerAttack.INSTANCE.talking = false;
        }
    }

}
