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
        animator.ResetTrigger("Normal");
        animator.SetTrigger("Hover");
    }

    void OnMouseExit(){
        animator.ResetTrigger("Hover");
        animator.SetTrigger("Normal");
    }
}
