using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using System.Text;
using System.Threading;

public class OtherPlayerManager : MonoBehaviour
{

    public GameObject gameOver;
    public int uploadGameMapGap = 1; 

    // 上传所有。用于给新登录者
    public void uploadMapAll()
    {
        StringBuilder SB = new StringBuilder(); 
        GameObject catGO = GameObject.FindGameObjectWithTag("cat");
        GameObject fishGO = GameObject.FindGameObjectWithTag("fish");

        SB.Append(catGO.transform.position.x); SB.Append("|");
        SB.Append(catGO.transform.position.y); SB.Append("|");
        SB.Append(fishGO.transform.position.x); SB.Append("|");
        SB.Append(fishGO.transform.position.y);

        MyPlayer.tcpClient.sendMsg(CodeConfig.UPDATE_MAP_ALL, SB.ToString());    
    }
    // 上传自己的
    public void uploadMyMap()
    {
        StringBuilder SB = new StringBuilder();
        SB.Append(MyPlayer.playerGO.transform.position.x); SB.Append("|");
        SB.Append(MyPlayer.playerGO.transform.position.y);  
        MyPlayer.tcpClient.sendMsg(CodeConfig.UPDATE_OTHERS_MAP, SB.ToString());
    }
    public void updateMapAll(String s)
    {
        int i = 0;
        StringBuilder catXSB = new StringBuilder();

        for (; s[i] != '|'; ++i)
        {
            catXSB.Append(s[i]);
        }
        StringBuilder catYSB = new StringBuilder();
        for (++i; s[i] != '|'; ++i)
        {
            catYSB.Append(s[i]);
        }
        StringBuilder fishXSB = new StringBuilder();
        for( ++i; s[i] != '|'; ++i)
        {
            fishXSB.Append(s[i]);
        }
        StringBuilder fishYSB = new StringBuilder();
        for (++i; i<s.Length; ++i)
        {
            fishYSB.Append(s[i]);
        }
        Debug.Log("catXSB:" + catXSB.ToString());
        float catX = (float)Convert.ToDouble(catXSB.ToString());
        float catY = (float)Convert.ToDouble(catYSB.ToString());
        float fishX = (float)Convert.ToDouble(fishXSB.ToString());
        float fishY = (float)Convert.ToDouble(fishYSB.ToString());
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            MyPlayer.playerGO.transform.position = new Vector3(catX, catY, MyPlayer.playerGO.transform.position.z);
            GameObject fishGO = GameObject.FindGameObjectWithTag("fish");
            fishGO.transform.position = new Vector3(fishX, fishY, fishGO.transform.position.z);
        } else if (MyPlayer.playertype == Config.PLAYER_TYPE_FISH)
        {
            MyPlayer.playerGO.transform.position = new Vector3(fishX, fishY, MyPlayer.playerGO.transform.position.z);
            GameObject catGO = GameObject.FindGameObjectWithTag("cat");
            catGO.transform.position = new Vector3(catX, catY, catGO.transform.position.z);
             
        }
    }
    public void updateOthersMap(String s)
    {
        GameObject go;
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            go = GameObject.FindGameObjectWithTag("fish");
        } else
        {
            go = GameObject.FindGameObjectWithTag("cat");
        }
        int i = 0;
        StringBuilder SB = new StringBuilder();
        SB.Clear();
        for (; s[i] != '|'; ++i)
        {
            SB.Append(s[i]);
        }
        float otherX = (float)Convert.ToDouble(SB.ToString());
        SB.Clear();  
        for (++i; i < s.Length; ++i)
        {
            SB.Append(s[i]);
        } 
        float otherY = (float)Convert.ToDouble(SB.ToString());
        Debug.Log("xSB:" + otherX);
        Debug.Log("ySB:" + otherY); 
        go.transform.position = new Vector3(otherX, otherY, go.transform.position.z);
        
    }
    public void HandleMsg(MyJson myJson)
    { 
        if (myJson.roomid != MyPlayer.roomid)
        {
            return;
        }
        switch (myJson.type)
        {
            case CodeConfig.MOVE_HOR:
                if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
                {
                    GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().moveHor(Convert.ToInt32(myJson.msg));
                } else
                { 
                    GameObject.FindGameObjectWithTag("cat").GetComponent<CatPlayer>().moveHor(Convert.ToInt32(myJson.msg));
                }
                break;
            case CodeConfig.MOVE_VER: 
                GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().moveVer(Convert.ToInt32(myJson.msg));
                break;
            case CodeConfig.SKILL:
                GameObject.FindGameObjectWithTag("cat").GetComponent<CatPlayer>().skill();
                break;
            case CodeConfig.NEW_CLIENT_CAT: 
                uploadMapAll();
                break;
            case CodeConfig.NEW_CLIENT_FISH:
                uploadMapAll();
                break;
            case CodeConfig.UPDATE_MAP_ALL:
                updateMapAll(myJson.msg);
                break;
            case CodeConfig.UPDATE_OTHERS_MAP:
                updateOthersMap(myJson.msg);
                break;
            case CodeConfig.GAME_OVER:
                GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().gameOverGO.SetActive(true);
                break;
            case CodeConfig.FISH_WIN:
                GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().fishwin.SetActive(true);
                break;
            case CodeConfig.SKILL_FISH:
                GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().skill();
                break;
        } 
        
    }
    private void Awake()
    {
        
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { 
    }
}
