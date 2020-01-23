using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChatBtn : MonoBehaviour
{
    public void playAudio()
    {
        Debug.Log("play audio");
        GameObject.FindGameObjectWithTag("audiochat").GetComponent<AudioChat>().aud.Play();
        //GameObject.FindGameObjectWithTag("audiochat").GetComponent<AudioChat>().endAudios();  
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
