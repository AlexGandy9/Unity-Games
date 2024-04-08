using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    private Animator animator;
    void Start(){
        animator = GetComponent<Animator>();
    }
    void OnMouseOver(){
        print("WEFDSSSWR");
        animator.ResetTrigger("Normal");
        animator.SetTrigger("Hover");
    }

    void OnMouseExit(){
        print("FSDG");
        animator.ResetTrigger("Hover");
        animator.SetTrigger("Normal");
    }
}
