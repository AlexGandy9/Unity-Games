using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthText : MonoBehaviour
{

    public GameObject tracker;

    public float timeToLive = 10000000f;
    public float floatSpeed = 500;
    public TextMeshPro textMesh;
    public Vector3 floatDirection = new Vector3(0,1,0);

    RectTransform rTransform;

    float timeElapsed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        rTransform = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rTransform.position = tracker.transform.position;

        timeElapsed += Time.deltaTime;
        // rTransform.position += floatSpeed * floatDirection * Time.deltaTime;
        if(timeElapsed > timeToLive){
            // Destroy(gameObject);
        }
        
    }
}
