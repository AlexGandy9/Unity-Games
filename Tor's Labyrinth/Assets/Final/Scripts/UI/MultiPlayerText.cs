using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerText : MonoBehaviour
{
    public GameObject mpText;
    public Animator mpAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mpText.SetActive(false);   
    }

    public void OnPointerOver(){
        mpText.SetActive(true);
        mpAnimator.SetBool("Hover", true);
    }

    public void OnPointerExit(){
        mpText.SetActive(false);
        mpAnimator.SetBool("Hover", false);
    }
}
