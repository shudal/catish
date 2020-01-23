using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public void moveHor(int h)
    {
        Debug.Log("move controller, move hor:" + h);
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        { 
            MyPlayer.playerGO.GetComponent<CatPlayer>().moveHor(h); 
        } else if (MyPlayer.playertype == Config.PLAYER_TYPE_FISH)
        {
            MyPlayer.playerGO.GetComponent<FishPlayer>().moveHor(h);
        }
        MyPlayer.tcpClient.sendMsg(CodeConfig.MOVE_HOR, h + "");
    }
    public void moveVer(int h)
    {
        Debug.Log("move ver, h:" + h);
        if (MyPlayer.playertype ==Config.PLAYER_TYPE_FISH)
        {
            MyPlayer.playerGO.GetComponent<FishPlayer>().moveVer(h);
        }
        MyPlayer.tcpClient.sendMsg(CodeConfig.MOVE_VER, h + "");
    }
    public void skill()
    {
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            MyPlayer.playerGO.GetComponent<CatPlayer>().skill(); 
            MyPlayer.tcpClient.sendMsg(CodeConfig.SKILL, "");
        } else
        {
            MyPlayer.playerGO.GetComponent<FishPlayer>().skill();
            MyPlayer.tcpClient.sendMsg(CodeConfig.SKILL_FISH, "");
        }
    }
    IEnumerator uploadTheGameMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            MyPlayer.otherPlayerManager.uploadMyMap();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(uploadTheGameMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
