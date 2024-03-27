using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [TextArea(3,10)]
    public string[] text;
    public string[] choices;

    public string pref;

    public bool usePref = false;


    public string choice;

    public string[] getText(){
        return text;
    }

    public string[] getChoices(){
        return choices;
    }

    public string getPref(){
        return pref;
    }

    public bool getUsePref(){
        return usePref;
    }
}
