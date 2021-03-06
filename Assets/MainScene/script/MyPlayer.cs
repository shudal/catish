﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public static string roomid;
    public static int playertype;
    public GameObject controllerGO;
    public static GameObject playerGO;
    public static OtherPlayerManager otherPlayerManager;
    public static TcpClient tcpClient;
    public static GameObject myGameGO;
    public static GameObject catGO;
    public static GameObject fishGO;
    public static GameObject audioChatGO;
    private void Awake()
    {
        if (playertype == Config.PLAYER_TYPE_CAT)
        {
            playerGO = GameObject.FindGameObjectWithTag("cat"); 
        } else if (playertype == Config.PLAYER_TYPE_FISH)
        {
            playerGO = GameObject.FindGameObjectWithTag("fish");
        }
        catGO = GameObject.FindGameObjectWithTag("cat");
        fishGO = GameObject.FindGameObjectWithTag("fish");
        otherPlayerManager = GameObject.FindWithTag("OtherPlayerManagerGO").GetComponent<OtherPlayerManager>();
        tcpClient = GameObject.FindGameObjectWithTag("tcpclient").GetComponent<TcpClient>();
        audioChatGO = GameObject.FindGameObjectWithTag("audiochat");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("roomid:" + roomid);
        Debug.Log("playertype:" + playertype);
        controllerGO.GetComponent<Controller>().myInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
