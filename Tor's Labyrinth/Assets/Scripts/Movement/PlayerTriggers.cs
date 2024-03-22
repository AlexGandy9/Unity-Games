using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggers : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Player hit enemy");
        SceneManager.LoadScene("SampleScene");
    }
}
