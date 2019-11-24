using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterGame : MonoBehaviour
{
    public Text roomidText;
    public int playertype;

    public void StartGame()
    {
        MyPlayer.playertype = playertype;
        MyPlayer.roomid = roomidText.text;
        SceneManager.LoadSceneAsync("MainScene");
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
