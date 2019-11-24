using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn : MonoBehaviour
{ 
    public void returnToEntry()
    {
        Debug.Log("ready to load entry scene");
        SceneManager.LoadSceneAsync("Entry");
        Debug.Log("return to entry");
        MyPlayer.tcpClient.quitGame();
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
