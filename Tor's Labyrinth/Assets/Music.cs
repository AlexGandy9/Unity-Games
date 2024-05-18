using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private AudioSource winMusic;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (CharacterControllerMovement.winScreen.gameObject.active && !winMusic.loop){
            winMusic.loop = true;
            winMusic.Play();
            mainMusic.Stop();
        }
    }
}
