using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SinglePlayerText : MonoBehaviour
{
    public GameObject spText;
    public Animator spAnimator;
    // Start is called before the first frame update
    void Start()
    {
        spText.SetActive(false);
    }

    public void OnPointerOver(){
        spText.SetActive(true);
        spAnimator.SetBool("Hover", true);
    }

    public void OnPointerExit(){
        spText.SetActive(false);
        spAnimator.SetBool("Hover", false);
    }
}
