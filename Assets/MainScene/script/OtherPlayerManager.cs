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

    Queue<Audios> audioBytesQueue = new Queue<Audios>();

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
        int i = 0;
        StringBuilder SB3 = new StringBuilder();
        SB3.Clear();
        for (; s[i] != '|'; ++i)
        {
            SB3.Append(s[i]);
        } 
        float otherX = (float)Convert.ToDouble(SB3.ToString());
        SB3.Clear();  
        for (++i; i < s.Length && s[i] != '\n'; ++i)
        {
            SB3.Append(s[i]);
        } 
        float otherY = (float)Convert.ToDouble(SB3.ToString());
        SB3.Clear(); 
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            MyPlayer.fishGO.transform.position = new Vector3(otherX, otherY, MyPlayer.fishGO.transform.position.z);
        }
        else
        { 
            MyPlayer.catGO.transform.position = new Vector3(otherX, otherY, MyPlayer.catGO.transform.position.z);
        } 
        
    }
    void handleAudio(string auS)
    {
        new Thread(() =>
        {
            int i = 0;
            StringBuilder ss1 = new StringBuilder();
            for (; auS[i] != '|'; ++i)
            {
                ss1.Append(auS[i]);
            }
            int c = Convert.ToInt32(ss1.ToString());
            ss1.Clear();
            for (++i; i < auS.Length; ++i)
            {
                ss1.Append(auS[i]);
            }
            //byte[] ba = Convert.FromBase64String(ss1.ToString());
            Debug.Log("ba str:" + ss1.ToString());
            byte[] ba = Base32.FromBase32String(ss1.ToString());
            audioBytesQueue.Enqueue(new Audios(ba, c));
        }).Start();
       
    }
    public void HandleMsg(MyJson myJson)
    { 
        if (myJson.roomid != MyPlayer.roomid)
        {
            return;
        }
        switch (myJson.type)
        { 
            case CodeConfig.UPDATE_OTHERS_MAP:
                updateOthersMap(myJson.msg);
                break;
            case CodeConfig.AUDIO:
                handleAudio(myJson.msg);
                break;
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
        if (audioBytesQueue.Count > 0)
        {
            HandleAudiosBytes((audioBytesQueue.Dequeue()));
        }
    }
    void HandleAudiosBytes(Audios a)
    { 
        MyPlayer.audioChatGO.GetComponent<AudioChat>().playByte(a.audioBytes, a.chan);
    }
}

class Audios
{
    public byte[] audioBytes;
    public int chan;
    public Audios(byte[] ba, int c)
    {
        audioBytes = ba;
        chan = c;
    }
}