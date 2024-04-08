using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float spaces;
    [SerializeField] private Slider spacesLeft;
    public Canvas canvas;
    private float spacesMax = 10;

    // Start is called before the first frame update
    void Start()
    {
        SetSpaces(spaces);
    }

    void Update(){
        if (spaces < 10){
            spaces += 0.001f;
            UpdateSpace();
        }
    }

    public void UpdateSpace(){
        spacesLeft.value = spaces / spacesMax;
    }

    public void RemoveSpace(){
        if (spaces > 0){
            spaces--;
            spacesLeft.value = spaces / spacesMax;
        }

        if (spaces <= 0){
            RemoveUI();
        }
    }

    public void SetSpaces(float s){
        spacesLeft.value = s;

        if (s > spacesMax){
            spaces = spacesMax;
        }
    }

    private void RemoveUI(){
        canvas.gameObject.active = false;
        SetSpaces(10);
    }
}
